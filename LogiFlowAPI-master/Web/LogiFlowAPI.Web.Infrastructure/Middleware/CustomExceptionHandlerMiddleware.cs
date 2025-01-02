namespace LogiFlowAPI.Web.Infrastructure.Middleware
{
    using System.Diagnostics;
    using System.Text.Json;
    using System.Threading.Tasks;

    using LogiFlowAPI.Common.Exceptions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;

        public CustomExceptionHandlerMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await this.next(context);
            }
            catch (HttpException ex)
            {
                ProblemDetails problemDetails = new()
                {
                    Title = ex.Message,
                    Status = ex.StatusCode,
                    Extensions = { ["traceId"] = Activity.Current?.Id?.ToString() ?? context.TraceIdentifier },
                };

                await context.Response.WriteAsJsonAsync(problemDetails, new JsonSerializerOptions { }, "application/problem+json");
            }
        }
    }
}
