using Autofac.Extensions.DependencyInjection;
using DinkToPdf;
using DinkToPdf.Contracts;
using Project.Admin.Hubs;
using Project.Core.Configuration;
using Project.Core.Infrastructure;
using Project.Web.Framework.Infrastructure.Extensions;
using QuestPDF.Infrastructure;

namespace Project.Admin;

public partial class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddJsonFile(ProjectConfigurationDefaults.AppSettingsFilePath, true, true);
        if (!string.IsNullOrEmpty(builder.Environment?.EnvironmentName))
        {
            var path = string.Format(ProjectConfigurationDefaults.AppSettingsEnvironmentFilePath, builder.Environment.EnvironmentName);
            builder.Configuration.AddJsonFile(path, true, true);
        }
        builder.Configuration.AddEnvironmentVariables();
        builder.Services.AddSignalR();
        //load application settings
        builder.Services.ConfigureApplicationSettings(builder);
        builder.Services.AddSingleton<IConverter>(
    new SynchronizedConverter(new PdfTools()));
        var appSettings = Singleton<AppSettings>.Instance;
        var useAutofac = appSettings.Get<CommonConfig>().UseAutofac;

        if (useAutofac)
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        else
            builder.Host.UseDefaultServiceProvider(options =>
            {
                //we don't validate the scopes, since at the app start and the initial configuration we need 
                //to resolve some services (registered as "scoped") through the root container
                options.ValidateScopes = false;
                options.ValidateOnBuild = true;
            });

        //add services to the application and configure service provider
        builder.Services.ConfigureApplicationServices(builder);

        QuestPDF.Settings.License = LicenseType.Community;
        var app = builder.Build();
        app.MapHub<ChatHub>("/chatHub");
        //configure the application HTTP request pipeline
        app.ConfigureRequestPipeline();
        await app.StartEngineAsync();

        await app.RunAsync();
    }
}