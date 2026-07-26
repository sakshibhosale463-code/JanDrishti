using FluentMigrator;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Conventions;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.Processors;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Project.Core.Configuration;
using Project.Core.Infrastructure;
using Project.Data.Extensions;
using Project.Data.Migrations;

namespace Project.Data;

/// <summary>
/// Represents object for the configuring DB context on application startup
/// </summary>
public partial class ProjectDbStartup : IProjectStartup
{
    /// <summary>
    /// Add and configure any of the middleware
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <param name="configuration">Configuration of the application</param>
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        var typeFinder = Singleton<ITypeFinder>.Instance;
        var mAssemblies = typeFinder.FindClassesOfType<MigrationBase>()
            .Select(t => t.Assembly)
            .Where(assembly => !assembly.FullName.Contains("FluentMigrator.Runner"))
            .Distinct()
            .ToArray();

        services
            // add common FluentMigrator services
            .AddFluentMigratorCore()
            .AddScoped<IProcessorAccessor, ProjectProcessorAccessor>()
            // set accessor for the connection string
            .AddScoped<IConnectionStringAccessor>(x => DataSettingsManager.LoadSettings())
            .AddScoped<IMigrationManager, MigrationManager>()
            .AddSingleton<IConventionSet, ProjectConventionSet>()
            .ConfigureRunner(rb =>
                rb.WithVersionTable(new MigrationVersionInfo()).AddSqlServer().AddMySql5().AddPostgres()
                    // define the assembly containing the migrations
                    .ScanIn(mAssemblies).For.Migrations().SetCommandTimeout());

        services.AddTransient(p => new Lazy<IVersionLoader>(p.GetRequiredService<IVersionLoader>()));

        //data layer 
        services.AddTransient<IDataProviderManager, DataProviderManager>();
        services.AddTransient(serviceProvider =>
            serviceProvider.GetRequiredService<IDataProviderManager>().DataProvider);

        //repositories	
        services.AddScoped(typeof(IRepository<>), typeof(EntityRepository<>));

        if (!DataSettingsManager.IsDatabaseInstalled())
            return;

        using var scope = services.BuildServiceProvider().CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationManager>();
        foreach (var assembly in mAssemblies)
            runner.ApplyUpSchemaMigrations(assembly);
    }

    /// <summary>
    /// Configure the using of added middleware
    /// </summary>
    /// <param name="application">Builder for configuring an application's request pipeline</param>
    public void Configure(IApplicationBuilder application)
    {
        var config = Singleton<AppSettings>.Instance.Get<CacheConfig>();

        LinqToDB.Common.Configuration.Linq.DisableQueryCache = config.LinqDisableQueryCache;
    }

    /// <summary>
    /// Gets order of this startup configuration implementation
    /// </summary>
    public int Order => 10;
}