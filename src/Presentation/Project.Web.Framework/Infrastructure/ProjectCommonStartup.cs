using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Project.Core.Infrastructure;
using Project.Web.Framework.Infrastructure.Extensions;

namespace Project.Web.Framework.Infrastructure;

/// <summary>
/// Represents object for the configuring common features and middleware on application startup
/// </summary>
public class ProjectCommonStartup : IProjectStartup
{
    /// <summary>
    /// Add and configure any of the middleware
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <param name="configuration">Configuration of the application</param>
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        //add options feature
        services.AddOptions();

        //add distributed cache
        services.AddDistributedCache();

        //add default HTTP clients
        services.AddProjectHttpClients();
    }

    /// <summary>
    /// Configure the using of added middleware
    /// </summary>
    /// <param name="application">Builder for configuring an application's request pipeline</param>
    public void Configure(IApplicationBuilder application)
    {
        //check whether database is installed
        application.UseInstallUrl();

        //configure PDF
        application.UseProjectPdf();
    }

    /// <summary>
    /// Gets order of this startup configuration implementation
    /// </summary>
    public int Order => 100; //common services should be loaded after error handlers
}
