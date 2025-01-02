namespace LogiFlowAPI.Web.Infrastructure.Middleware
{
    using Microsoft.AspNetCore.Builder;

    public static class CustomMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
        }
    }
}
