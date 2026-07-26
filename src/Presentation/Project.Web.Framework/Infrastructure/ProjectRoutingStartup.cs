using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Project.Core.Configuration;
using Project.Core.Infrastructure;

namespace Project.Web.Framework.Infrastructure;

/// <summary>
/// Represents object for the configuring routing on application startup
/// </summary>
public partial class NopRoutingStartup : IProjectStartup
{
    /// <summary>
    /// Add and configure any of the middleware
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <param name="configuration">Configuration of the application</param>
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        //cors domains
        var appSettings = Singleton<AppSettings>.Instance;
        var commonConfig = appSettings.Get<CommonConfig>();
        ArgumentException.ThrowIfNullOrWhiteSpace(commonConfig.CorsDomains);

        var domains = commonConfig.CorsDomains.Split(",", StringSplitOptions.RemoveEmptyEntries);
        if (domains == null || domains.Length == 0)
            ArgumentException.ThrowIfNullOrEmpty(commonConfig.CorsDomains);

        services.AddCors(options =>
        {
            options.AddPolicy("Policy",
            builder =>
            {
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowCredentials();
                builder.WithOrigins(domains);
            });
        });
    }

    /// <summary>
    /// Configure the using of added middleware
    /// </summary>
    /// <param name="application">Builder for configuring an application's request pipeline</param>
    public void Configure(IApplicationBuilder application)
    {
        //add the RoutingMiddleware
        application.UseRouting();

        //cors
        application.UseCors("Policy");

        var commonConfig = Singleton<AppSettings>.Instance.Get<CommonConfig>();
        if (commonConfig.PermitLimit > 0)
            application.UseRateLimiter();
    }

    /// <summary>
    /// Gets order of this startup configuration implementation
    /// </summary>
    public int Order => 400; // Routing should be loaded before authentication
}