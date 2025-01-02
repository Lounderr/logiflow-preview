namespace LogiFlowAPI.Web.Infrastructure.Extensions
{
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Text.Json.Serialization;

    using LogiFlowAPI.Data;
    using LogiFlowAPI.Data.Common;
    using LogiFlowAPI.Data.Common.Repositories;
    using LogiFlowAPI.Data.Models;
    using LogiFlowAPI.Data.Repositories;
    using LogiFlowAPI.Services.Interfaces.ServiceLifetimes;
    using LogiFlowAPI.Services.Messaging;
    using LogiFlowAPI.Web.Infrastructure.ParameterTransformers;
    using LogiFlowAPI.Web.Models;

    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ApplicationModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<LogiFlowDbContext>(options => options.UseSqlServer(connectionString));

            // Specifying the environment is not needed
            services.AddDatabaseDeveloperPageExceptionFilter();

            return services;
        }

        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, Role>(IdentityOptionsProvider.GetIdentityOptions)
                .AddEntityFrameworkStores<LogiFlowDbContext>();
            //.AddDefaultTokenProviders();

            return services;
        }

        /// <summary>
        /// Explicitly register services that are not in the same assembly as the service lifetime interfaces.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterServicesExplicitly(this IServiceCollection services)
        {
            //builder.Services.AddSingleton(builder.Configuration);

            services.AddTransient<IEmailSender, NullMessageSender>();

            return services;
        }

        /// <summary>
        /// Discovers and registers services that implement the ITransientService, ISingletonService or IScopedService interfaces.
        /// The service needs to be in the same assembly as the interface.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection DiscoverAndRegisterServices(this IServiceCollection services)
        {
            var serviceInterfaceType = typeof(ITransientService);
            var singletonServiceInterfaceType = typeof(ISingletonService);
            var scopedServiceInterfaceType = typeof(IScopedService);

            var types = serviceInterfaceType
                .Assembly
                    .GetExportedTypes()
                    .Where(t => t.IsClass && !t.IsAbstract)
                    .Select(t => new
                    {
                        Service = t.GetInterface($"I{t.Name}"),
                        Implementation = t
                    })
                    .Where(t => t.Service != null);

            foreach (var type in types)
            {
                if (serviceInterfaceType.IsAssignableFrom(type.Service))
                {
                    services.AddTransient(type.Service, type.Implementation);
                }
                else if (singletonServiceInterfaceType.IsAssignableFrom(type.Service))
                {
                    services.AddSingleton(type.Service, type.Implementation);
                }
                else if (scopedServiceInterfaceType.IsAssignableFrom(type.Service))
                {
                    services.AddScoped(type.Service, type.Implementation);
                }
            }

            return services;
        }

        public static IServiceCollection AddApiControllers(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
                //options.Filters.Add<MyCustomActionFilter>();
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

                // Replaces repeated instances of an object with $ref properties to prevent infinite circular references
                // options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            });

            return services;
        }

        public static IServiceCollection ConfigureInvalidModelStateResponse(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
             {
                 options.InvalidModelStateResponseFactory = context =>
                 {
                     var problemDetails = new ValidationProblemDetails(context.ModelState)
                     {
                         Title = "One or more validation errors occurred.",
                         Status = StatusCodes.Status400BadRequest,
                     };

                     problemDetails.Extensions["traceId"] = Activity.Current?.Id ?? context.HttpContext?.TraceIdentifier;

                     return new BadRequestObjectResult(problemDetails)
                     {
                         ContentTypes = { "application/problem+json" }
                     };
                 };
             });
            return services;
        }

        public static IServiceCollection AddDataRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            return services;
        }

        public static TokenSettings GetApplicationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            // These settings can be accessed in the controllers via the IOptions<T> pattern

            var tokenSettingsConfig = configuration.GetSection(nameof(TokenSettings));

            services.Configure<TokenSettings>(tokenSettingsConfig);

            return tokenSettingsConfig.Get<TokenSettings>();
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, TokenSettings tokenSettings)
        {
            TokenValidationParameters authTokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidIssuer = tokenSettings.Issuer,

                ValidateAudience = true,
                ValidAudience = tokenSettings.Audience,

                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenSettings.Secret)),
            };

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    var securityKey = Encoding.ASCII.GetBytes(tokenSettings.Secret);

                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = authTokenValidationParameters;
                });

            return services;
        }

        public static IServiceCollection AddCors(this IServiceCollection services, IConfiguration configuration)
        {
            var corsOrigins = configuration["CorsAllowedOrigins"].Split(",", System.StringSplitOptions.RemoveEmptyEntries);

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder => builder
                        .WithOrigins(corsOrigins)
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            return services;
        }
    }
}
