using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Project.Core.Infrastructure;
using Project.Web.Framework.Infrastructure.Extensions;

namespace Project.Web.Framework.Infrastructure;

/// <summary>
/// Represents object for the configuring authentication middleware on application startup
/// </summary>
public partial class AuthenticationStartup : IProjectStartup
{
    /// <summary>
    /// Add and configure any of the middleware
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <param name="configuration">Configuration of the application</param>
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        //add data protection
        services.AddNopDataProtection();

        //add authentication
        services.AddProjectAuthentication();
    }

    /// <summary>
    /// Configure the using of added middleware
    /// </summary>
    /// <param name="application">Builder for configuring an application's request pipeline</param>
    public void Configure(IApplicationBuilder application)
    {
        //configure authentication
        application.UseProjectAuthentication();
    }

    /// <summary>
    /// Gets order of this startup configuration implementation
    /// </summary>
    public int Order => 500; //These middleware are placed between UseRouting and UseEndpoints
}