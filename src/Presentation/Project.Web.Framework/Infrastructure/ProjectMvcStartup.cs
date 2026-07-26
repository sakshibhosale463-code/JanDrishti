using Asp.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Project.Core.Infrastructure;
using Project.Web.Framework.Infrastructure.Extensions;

namespace Project.Web.Framework.Infrastructure;

/// <summary>
/// Represents object for the configuring MVC on application startup
/// </summary>
public partial class ProjectMvcStartup : IProjectStartup
{
    /// <summary>
    /// Add and configure any of the middleware
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <param name="configuration">Configuration of the application</param>
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        //add and configure MVC feature
        services.AddProjectMvc();

        services.AddSwagger();

        //versioning
        services.AddApiVersioning(option =>
        {
            option.ReportApiVersions = true;
            option.DefaultApiVersion = new ApiVersion(1, 0);
            option.AssumeDefaultVersionWhenUnspecified = true;
        });
    }

    /// <summary>
    /// Configure the using of added middleware
    /// </summary>
    /// <param name="application">Builder for configuring an application's request pipeline</param>
    public void Configure(IApplicationBuilder application)
    {
        application.UseSwaggerpoints();
    }

    /// <summary>
    /// Gets order of this startup configuration implementation
    /// </summary>
    public int Order => 1000; //MVC should be loaded last
}