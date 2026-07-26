using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Project.Core.Infrastructure;
using Project.Web.Framework.Infrastructure.Extensions;

namespace Project.Web.Framework.Infrastructure;

/// <summary>
/// Represents class for the configuring routing on application startup
/// </summary>
public partial class ProjectStaticFilesStartup : IProjectStartup
{
    /// <summary>
    /// Add and configure any of the middleware
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <param name="configuration">Configuration of the application</param>
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        //compression
        services.AddResponseCompression();
    }

    /// <summary>
    /// Configure the using of added middleware
    /// </summary>
    /// <param name="application">Builder for configuring an application's request pipeline</param>
    public void Configure(IApplicationBuilder application)
    {
        //use response compression before UseNopStaticFiles to support compress for it
        application.UseNopResponseCompression();

        //use static files feature
        application.UseProjectStaticFiles();
    }

    /// <summary>
    /// Gets order of this startup configuration implementation
    /// </summary>
    public int Order => 99; //Static files should be registered before routing & custom middlewares
}