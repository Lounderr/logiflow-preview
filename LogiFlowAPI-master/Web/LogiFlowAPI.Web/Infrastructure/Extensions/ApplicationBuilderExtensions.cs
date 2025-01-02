namespace LogiFlowAPI.Web.Infrastructure.Extensions
{
    using System.Diagnostics;
    using System.Reflection;

    using LogiFlowAPI.Services.Mapping;
    using LogiFlowAPI.Web.Models;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Hosting;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder ConfigureForEnvironment(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwaggerDocumentation();

                // Return stack trace on 500 for development
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();

                // Return problem details on 500 for production
                UseInternalServerErrorProblemDetails(app);
            }

            return app;
        }

        public static IApplicationBuilder UseNotFoundProblemDetails(this IApplicationBuilder app)
        {
            // Return friendly error on 404
            app.UseStatusCodePages(async context =>
            {
                if (context.HttpContext.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    context.HttpContext.Response.ContentType = "application/problem+json";

                    var problemDetails = new ProblemDetails
                    {
                        Title = "The requested URL was not found.",
                        Status = 404,
                    };

                    problemDetails.Extensions["traceId"] = Activity.Current?.Id ?? context.HttpContext?.TraceIdentifier;

                    await context.HttpContext.Response.WriteAsJsonAsync(problemDetails);
                }
            });

            return app;
        }

        public static IApplicationBuilder UseAutoMapper(this IApplicationBuilder app)
        {
            AutoMapperConfig.RegisterMappings(typeof(TokenSettings).GetTypeInfo().Assembly);

            return app;
        }

        public static IApplicationBuilder UseEndpoints(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            return app;
        }

        private static IApplicationBuilder UseInternalServerErrorProblemDetails(IApplicationBuilder app)
        {
            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/problem+json";

                    var problemDetails = new ProblemDetails
                    {
                        Title = "An internal server error occurred.",
                        Status = 500,
                    };

                    problemDetails.Extensions["traceId"] = Activity.Current?.Id ?? context.TraceIdentifier;

                    await context.Response.WriteAsJsonAsync(problemDetails);
                });
            });

            return app;
        }
    }
}
