namespace LogiFlowAPI.Web
{
    using System.Reflection;

    using LogiFlowAPI.Web.Infrastructure.Extensions;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services
                .AddCors(builder.Configuration)
                .AddDatabase(builder.Configuration)
                .AddIdentity()
                .AddSwaggerDocumentation()
                .AddJwtAuthentication(builder.Services.GetApplicationSettings(builder.Configuration))
                .AddHttpContextAccessor()
                .AddAutoMapper(Assembly.GetExecutingAssembly())
                .AddDataRepositories()
                .DiscoverAndRegisterServices()
                .RegisterServicesExplicitly()
                .AddApiControllers()
                .ConfigureInvalidModelStateResponse();

            var app = builder.Build();

            app
                .ConfigureForEnvironment(app.Environment)
                .InitializeDatabase()
                .UseAutoMapper()
                .UseCors()
                .UseHttpsRedirection()
                .UseStaticFiles()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints()
                .UseNotFoundProblemDetails();

            app.Run();
        }
    }
}
