using System.Reflection;
using System.Runtime.ExceptionServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using MimeKit;
using Project.Core;
using Project.Core.Configuration;
using Project.Core.Domain.Common;
using Project.Core.Infrastructure;
using Project.Data;
using Project.Data.Migrations;
using Project.Services.Installation;
using Project.Services.Logging;
using Project.Web.Framework.Mvc.Routing;
using QuestPDF.Drawing;

namespace Project.Web.Framework.Infrastructure.Extensions;

/// <summary>
/// Represents extensions of IApplicationBuilder
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Configure the application HTTP request pipeline
    /// </summary>
    /// <param name="application">Builder for configuring an application's request pipeline</param>
    public static void ConfigureRequestPipeline(this IApplicationBuilder application)
    {
        EngineContext.Current.ConfigureRequestPipeline(application);
    }

    public static Task StartEngineAsync(this IApplicationBuilder _)
    {
        var engine = EngineContext.Current;

        //further actions are performed only when the database is installed
        if (DataSettingsManager.IsDatabaseInstalled())
        {
            //initialize and start schedule tasks
            Services.Tasks.TaskManager.Instance.Initialize();
            Services.Tasks.TaskManager.Instance.Start();
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Adds the authentication middleware, which enables authentication capabilities.
    /// </summary>
    /// <param name="application">Builder for configuring an application's request pipeline</param>
    public static void UseProjectAuthentication(this IApplicationBuilder application)
    {
        //check whether database is installed
        if (!DataSettingsManager.IsDatabaseInstalled())
            return;

        application.UseMiddleware<AuthenticationMiddleware>();
    }

    /// <summary>
    /// Add exception handling
    /// </summary>
    /// <param name="application">Builder for configuring an application's request pipeline</param>
    public static void UseProjectExceptionHandler(this IApplicationBuilder application)
    {
        var appSettings = EngineContext.Current.Resolve<AppSettings>();
        var webHostEnvironment = EngineContext.Current.Resolve<IWebHostEnvironment>();
        var useDetailedExceptionPage = appSettings.Get<CommonConfig>().DisplayFullErrorStack || webHostEnvironment.IsDevelopment();
        if (useDetailedExceptionPage)
        {
            //get detailed exceptions for developing and testing purposes
            application.UseDeveloperExceptionPage();
        }
        else
        {
            //or use special exception handler
            application.UseExceptionHandler("/Error/Error");
        }

        //log errors
        application.UseExceptionHandler(handler =>
        {
            handler.Run(async context =>
            {
                var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                if (exception == null)
                    return;

                try
                {
                    //check whether database is installed
                    if (DataSettingsManager.IsDatabaseInstalled())
                    {
                        //get current customer
                        var currentCustomer = await EngineContext.Current.Resolve<IWorkContext>().GetCurrentUserAsync();

                        //log error
                        await EngineContext.Current.Resolve<ILogger>().ErrorAsync(exception.Message, exception, currentCustomer);
                    }
                }
                finally
                {
                    //rethrow the exception to show the error page
                    ExceptionDispatchInfo.Throw(exception);
                }
            });
        });
    }

    /// <summary>
    /// Configure middleware checking whether database is installed
    /// </summary>
    /// <param name="application">Builder for configuring an application's request pipeline</param>
    public static void UseInstallUrl(this IApplicationBuilder application)
    {
        application.UseMiddleware<InstallUrlMiddleware>();
    }

    /// <summary>
    /// Configure middleware for dynamically compressing HTTP responses
    /// </summary>
    /// <param name="application">Builder for configuring an application's request pipeline</param>
    public static void UseNopResponseCompression(this IApplicationBuilder application)
    {
        if (!DataSettingsManager.IsDatabaseInstalled())
            return;

        //whether to use compression (gzip by default)
        if (EngineContext.Current.Resolve<CommonSettings>().UseResponseCompression)
            application.UseResponseCompression();
    }

    /// <summary>
    /// Configure Endpoints routing
    /// </summary>
    /// <param name="application">Builder for configuring an application's request pipeline</param>
    public static void UseProjectEndpoints(this IApplicationBuilder application)
    {
        //Execute the endpoint selected by the routing middleware
        application.UseEndpoints(endpoints =>
        {
            //register all routes
            EngineContext.Current.Resolve<IRoutePublisher>().RegisterRoutes(endpoints);
        });
    }
    public static void UseSwaggerpoints(this IApplicationBuilder application)
    {
        // Enable middleware to serve generated Swagger as a JSON endpoint
        application.UseSwagger();

        // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
        // specifying the Swagger JSON endpoint.
        application.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Project API V1");
            c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
           
        });

     
    }

    /// <summary>
    /// Configure PDF
    /// </summary>
    public static void UseProjectPdf(this IApplicationBuilder _)
    {
        if (!DataSettingsManager.IsDatabaseInstalled())
            return;

        var fileProvider = EngineContext.Current.Resolve<IProjectFileProvider>();
        var fontPaths = fileProvider.EnumerateFiles(fileProvider.MapPath("~/App_Data/Pdf/"), "*.ttf") ?? Enumerable.Empty<string>();

        //write placeholder characters instead of unavailable glyphs for both debug/release configurations
        QuestPDF.Settings.CheckIfAllTextGlyphsAreAvailable = false;

        foreach (var fp in fontPaths)
        {
            FontManager.RegisterFont(File.OpenRead(fp));
        }
    }

    /// <summary>
    /// Configure static file serving
    /// </summary>
    /// <param name="application">Builder for configuring an application's request pipeline</param>
    public static void UseProjectStaticFiles(this IApplicationBuilder application)
    {
        var fileProvider = EngineContext.Current.Resolve<IProjectFileProvider>();
        var appSettings = EngineContext.Current.Resolve<AppSettings>();

        void staticFileResponse(StaticFileResponseContext context)
        {
            if (!string.IsNullOrEmpty(appSettings.Get<CommonConfig>().StaticFilesCacheControl))
                context.Context.Response.Headers.Append(HeaderNames.CacheControl, appSettings.Get<CommonConfig>().StaticFilesCacheControl);
        }

        //common static files
        application.UseStaticFiles(new StaticFileOptions { OnPrepareResponse = staticFileResponse });

        if (appSettings.Get<CommonConfig>().ServeUnknownFileTypes)
        {
            application.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(fileProvider.GetAbsolutePath(".well-known")),
                RequestPath = new PathString("/.well-known"),
                ServeUnknownFileTypes = true,
            });
        }
    }

}
