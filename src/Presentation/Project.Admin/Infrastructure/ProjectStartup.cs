using Project.Admin.Factories;
using Project.Admin.Infrastructure.Installation;
using Project.Admin.Infrastructure.Services;
using Project.Core.Infrastructure;

namespace Project.Admin.Infrastructure;

/// <summary>
/// Represents the registering services on application startup
/// </summary>
public partial class ProjectStartup : IProjectStartup
{
    /// <summary>
    /// Add and configure any of the middleware
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <param name="configuration">Configuration of the application</param>
    public virtual void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {



        //installation localization service
        services.AddScoped<IInstallationLocalizationService, InstallationLocalizationService>();
        services.AddScoped<IUserRoleModelFactory, UserRoleModelFactory>();
        services.AddScoped<IUserModelFactory, UserModelFactory>();
        services.AddScoped<IRegistrationModelFactory, RegistrationModelFactory>();
        services.AddHostedService<DailyTaskScheduler>();
    }

    /// <summary>
    /// Configure the using of added middleware
    /// </summary>
    /// <param name="application">Builder for configuring an application's request pipeline</param>
    public void Configure(IApplicationBuilder application)
    {
    }

    /// <summary>
    /// Gets order of this startup configuration implementation
    /// </summary>
    public int Order => 2002;
}