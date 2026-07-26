using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Project.Core.Infrastructure;
using Project.Core;
using Project.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.Threading.RateLimiting;
using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.DataProtection;
using Project.Core.Security;
using Project.Core.Http;
using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Newtonsoft.Json.Serialization;
using Project.Web.Framework.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Project.Web.Framework.Models;
using NUglify.Helpers;
using System.Net;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;

namespace Project.Web.Framework.Infrastructure.Extensions;

/// <summary>
/// Represents extensions of IServiceCollection
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Configure base application settings
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <param name="builder">A builder for web applications and services</param>
    public static void ConfigureApplicationSettings(this IServiceCollection services,
        WebApplicationBuilder builder)
    {
        //create default file provider
        CommonHelper.DefaultFileProvider = new ProjectFileProvider(builder.Environment);

        //register type finder
        var typeFinder = new WebAppTypeFinder();
        Singleton<ITypeFinder>.Instance = typeFinder;
        services.AddSingleton<ITypeFinder>(typeFinder);

        //bind general configuration
        services.BindApplicationSettings(builder);
    }

    /// <summary>
    /// Bind application settings
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <param name="builder">A builder for web applications and services</param>
    public static void BindApplicationSettings(this IServiceCollection services, WebApplicationBuilder builder)
    {
        var typeFinder = Singleton<ITypeFinder>.Instance;

        //add configuration parameters
        var configurations = typeFinder
            .FindClassesOfType<IConfig>()
            .Select(configType => (IConfig)Activator.CreateInstance(configType))
            .ToList();

        foreach (var config in configurations)
            builder.Configuration.GetSection(config.Name).Bind(config, options => options.BindNonPublicProperties = true);

        var appSettings = Singleton<AppSettings>.Instance;

        if (appSettings == null)
        {
            appSettings = AppSettingsHelper.SaveAppSettings(configurations, CommonHelper.DefaultFileProvider, false);
            services.AddSingleton(appSettings);
        }
        else
        {
            var needToUpdate = configurations.Any(conf => !appSettings.Configuration.ContainsKey(conf.Name));
            AppSettingsHelper.SaveAppSettings(configurations, CommonHelper.DefaultFileProvider, needToUpdate);
        }
    }

    /// <summary>
    /// Add services to the application and configure service provider
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <param name="builder">A builder for web applications and services</param>
    public static void ConfigureApplicationServices(this IServiceCollection services,
        WebApplicationBuilder builder)
    {
        //add accessor to HttpContext
        services.AddHttpContextAccessor();

        //create engine and configure service provider
        var engine = EngineContext.Create();

        builder.Services.AddRateLimiter(options =>
        {
            var settings = Singleton<AppSettings>.Instance.Get<CommonConfig>();

            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
                    factory: partition => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = settings.PermitLimit,
                        QueueLimit = settings.QueueCount,
                        Window = TimeSpan.FromMinutes(1)
                    }));

            options.RejectionStatusCode = settings.RejectionStatusCode;
        });

        engine.ConfigureServices(services, builder.Configuration);
    }

    /// <summary>
    /// Adds data protection services
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    public static void AddNopDataProtection(this IServiceCollection services)
    {
        var appSettings = Singleton<AppSettings>.Instance;
        if (appSettings.Get<AzureBlobConfig>().Enabled && appSettings.Get<AzureBlobConfig>().StoreDataProtectionKeys)
        {
            var blobServiceClient = new BlobServiceClient(appSettings.Get<AzureBlobConfig>().ConnectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(appSettings.Get<AzureBlobConfig>().DataProtectionKeysContainerName);
            var blobClient = blobContainerClient.GetBlobClient(ProjectDataProtectionDefaults.AzureDataProtectionKeyFile);

            var dataProtectionBuilder = services.AddDataProtection().PersistKeysToAzureBlobStorage(blobClient);

            if (!appSettings.Get<AzureBlobConfig>().DataProtectionKeysEncryptWithVault)
                return;

            var keyIdentifier = appSettings.Get<AzureBlobConfig>().DataProtectionKeysVaultId;
            var credentialOptions = new DefaultAzureCredentialOptions();
            var tokenCredential = new DefaultAzureCredential(credentialOptions);

            dataProtectionBuilder.ProtectKeysWithAzureKeyVault(new Uri(keyIdentifier), tokenCredential);
        }
        else
        {
            var dataProtectionKeysPath = CommonHelper.DefaultFileProvider.MapPath(ProjectDataProtectionDefaults.DataProtectionKeysPath);
            var dataProtectionKeysFolder = new System.IO.DirectoryInfo(dataProtectionKeysPath);

            //configure the data protection system to persist keys to the specified directory
            services.AddDataProtection().PersistKeysToFileSystem(dataProtectionKeysFolder);
        }
    }

    /// <summary>
    /// Adds authentication service
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    public static void AddProjectAuthentication(this IServiceCollection services)
    {
        var appSettings = Singleton<AppSettings>.Instance;
        var jwtConfig = appSettings.Get<JwtConfig>();

        //set default authentication schemes
        var authenticationBuilder = services.AddAuthentication(options =>
        {
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        });

        //add main jwt authentication
        authenticationBuilder.AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtConfig.Issuer,
                ValidAudience = jwtConfig.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key))
            };
        });
    }
    public static void AddSwagger(this IServiceCollection services)
    {
        // Add Swagger services
        services.AddSwaggerGen(c =>
        {
            //c.SwaggerDoc("v1", new OpenApiInfo { Title = "Project API", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });
    }

    /// <summary>
    /// Adds services required for distributed cache
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    public static void AddDistributedCache(this IServiceCollection services)
    {
        var appSettings = Singleton<AppSettings>.Instance;
        var distributedCacheConfig = appSettings.Get<DistributedCacheConfig>();

        if (!distributedCacheConfig.Enabled)
            return;

        switch (distributedCacheConfig.DistributedCacheType)
        {
            case DistributedCacheType.Memory:
                services.AddDistributedMemoryCache();
                break;

            case DistributedCacheType.SqlServer:
                services.AddDistributedSqlServerCache(options =>
                {
                    options.ConnectionString = distributedCacheConfig.ConnectionString;
                    options.SchemaName = distributedCacheConfig.SchemaName;
                    options.TableName = distributedCacheConfig.TableName;
                });
                break;

            case DistributedCacheType.Redis:
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = distributedCacheConfig.ConnectionString;
                    options.InstanceName = distributedCacheConfig.InstanceName ?? string.Empty;
                });
                break;

            case DistributedCacheType.RedisSynchronizedMemory:
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = distributedCacheConfig.ConnectionString;
                    options.InstanceName = distributedCacheConfig.InstanceName ?? string.Empty;
                });
                break;
        }
    }

    /// <summary>
    /// Add and configure default HTTP clients
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    public static void AddProjectHttpClients(this IServiceCollection services)
    {
        //default client
        services.AddHttpClient(NopHttpDefaults.DefaultHttpClient);
    }

    /// <summary>
    /// Add and configure MVC for the application
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <returns>A builder for configuring MVC services</returns>
    public static IMvcBuilder AddProjectMvc(this IServiceCollection services)
    {
        //add basic MVC feature
        var mvcBuilder = services.AddControllersWithViews();

        mvcBuilder.AddRazorRuntimeCompilation();
        services.AddRazorPages();

        //set some options
        mvcBuilder.AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.IncludeFields = true;
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });

        mvcBuilder.AddMvcOptions(options =>
        {
            //in .NET model binding for a non-nullable property may fail with an error message "The value '' is invalid"
            //here we set the locale name as the message, we'll replace it with the actual one later when not-null validation failed
            options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(_ => ProjectValidationDefaults.NotNullValidationLocaleName);
        });
        
        mvcBuilder.ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = (context) =>
            {
                var errorList = new List<ValidationErrorModel>();
                context.ModelState.ForEach(error =>
                {
                    var errorModel = new ValidationErrorModel
                    {
                        Key = error.Key.ToLower(),  
                    };
                    errorModel.Errors.AddRange(error.Value.Errors.Select(item => item.ErrorMessage));
                    errorList.Add(errorModel);
                });

                var responseModel = new BaseResponseModel<List<ValidationErrorModel>>
                {
                    Data = errorList,
                    Message = string.Empty,
                    Status = HttpStatusCode.BadRequest
                };
                return new OkObjectResult(responseModel);
            };
        });

        //add fluent validation
        services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

        //register all available validators from Project assemblies
        var assemblies = mvcBuilder.PartManager.ApplicationParts
            .OfType<AssemblyPart>()
            .Where(part => part.Name.StartsWith("Project", StringComparison.InvariantCultureIgnoreCase))
            .Select(part => part.Assembly);
        services.AddValidatorsFromAssemblies(assemblies);

        //register controllers as services, it'll allow to override them
        mvcBuilder.AddControllersAsServices();

        return mvcBuilder;
    }
}
