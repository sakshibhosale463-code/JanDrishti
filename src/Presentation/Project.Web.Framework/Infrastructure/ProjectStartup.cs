using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Project.Core;
using Project.Core.Caching;
using Project.Core.Configuration;
using Project.Core.Events;
using Project.Core.Http;
using Project.Core.Infrastructure;
using Project.Data.Mapping.Builders.SQS;
using Project.Services;
using Project.Services.Authentication;
using Project.Services.Caching;
using Project.Services.Catalog;
using Project.Services.Configuration;
using Project.Services.Email;
using Project.Services.Events;
using Project.Services.ExportImport;
using Project.Services.Helpers;
using Project.Services.Installation;
using Project.Services.Localization;
using Project.Services.Logging;
using Project.Services.Permissions;
using Project.Services.Registration;
using Project.Services.Report;
using Project.Services.Security;
using Project.Services.Tasks;
using Project.Services.Users;
using Project.Web.Framework.Mvc.Routing;

namespace Project.Web.Framework.Infrastructure;

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
        //file provider
        services.AddScoped<IProjectFileProvider, ProjectFileProvider>();

        //web helper
        services.AddScoped<IWebHelper, WebHelper>();

        //installation service
        services.AddScoped<IInstallationService, InstallationService>();

          services.AddScoped<IPermissionRecordService, PermissionRecordService>();
         services.AddScoped<ISQSLogService, SQSLogService>();
        
  

        //static cache manager
        var appSettings = Singleton<AppSettings>.Instance;
        var distributedCacheConfig = appSettings.Get<DistributedCacheConfig>();

        services.AddTransient(typeof(IConcurrentCollection<>), typeof(ConcurrentTrie<>));

        services.AddSingleton<ICacheKeyManager, CacheKeyManager>();
        services.AddScoped<IShortTermCacheManager, PerRequestCacheManager>();

        if (distributedCacheConfig.Enabled)
        {
            switch (distributedCacheConfig.DistributedCacheType)
            {
                case DistributedCacheType.Memory:
                    services.AddScoped<IStaticCacheManager, MemoryDistributedCacheManager>();
                    services.AddScoped<ICacheKeyService, MemoryDistributedCacheManager>();
                    break;
                case DistributedCacheType.SqlServer:
                    services.AddScoped<IStaticCacheManager, MsSqlServerCacheManager>();
                    services.AddScoped<ICacheKeyService, MsSqlServerCacheManager>();
                    break;
                case DistributedCacheType.Redis:
                    services.AddSingleton<IRedisConnectionWrapper, RedisConnectionWrapper>();
                    services.AddScoped<IStaticCacheManager, RedisCacheManager>();
                    services.AddScoped<ICacheKeyService, RedisCacheManager>();
                    break;
                case DistributedCacheType.RedisSynchronizedMemory:
                    services.AddSingleton<IRedisConnectionWrapper, RedisConnectionWrapper>();
                    services.AddSingleton<ISynchronizedMemoryCache, RedisSynchronizedMemoryCache>();
                    services.AddSingleton<IStaticCacheManager, SynchronizedMemoryCacheManager>();
                    services.AddScoped<ICacheKeyService, SynchronizedMemoryCacheManager>();
                    break;
            }

            services.AddSingleton<ILocker, DistributedCacheLocker>();
        }
        else
        {
            services.AddSingleton<ILocker, MemoryCacheLocker>();
            services.AddSingleton<IStaticCacheManager, MemoryCacheManager>();
            services.AddScoped<ICacheKeyService, MemoryCacheManager>();
        }

        //work context
        services.AddScoped<IWorkContext, WebWorkContext>();

        services.AddScoped<ILogger, DefaultLogger>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IImportManager, ImportManager>();
        services.AddScoped<ISettingService, SettingService>();
        services.AddScoped<IExportManager, ExportManager>();
        services.AddSingleton<IEventPublisher, EventPublisher>();
        services.AddSingleton<IRoutePublisher, RoutePublisher>();
        services.AddScoped<IDateTimeHelper, DateTimeHelper>();
        services.AddScoped<IUserRoleService, UserRoleService>();
        services.AddScoped<ILanguageService, LanguageService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<ILocalizationService, LocalizationService>();
        services.AddScoped<IScheduleTaskService, ScheduleTaskService>();
        services.AddScoped<ILocalizedEntityService, LocalizedEntityService>();
        services.AddScoped<IAuthenticationService, JwtAuthenticationService>();
        services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        services.AddSingleton<IRegistrationMasterService, RegistrationMasterService>();
        services.AddSingleton<IUserRoleService, UserRoleService>();
        services.AddSingleton<IReportService, ReportService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IHttpService, HttpService>();


        //register all settings
        var typeFinder = Singleton<ITypeFinder>.Instance;

        var settings = typeFinder.FindClassesOfType(typeof(ISettings)).ToList();
        foreach (var setting in settings)
        {
            services.AddScoped(setting, serviceProvider =>
            {
                return serviceProvider.GetRequiredService<ISettingService>().LoadSettingAsync(setting).Result;
            });
        }


        //event consumers
        var consumers = typeFinder.FindClassesOfType(typeof(IConsumer<>)).ToList();
        foreach (var consumer in consumers)
            foreach (var findInterface in consumer.FindInterfaces((type, criteria) =>
            {
                var isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                return isMatch;
            }, typeof(IConsumer<>)))
                services.AddScoped(findInterface, consumer);
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