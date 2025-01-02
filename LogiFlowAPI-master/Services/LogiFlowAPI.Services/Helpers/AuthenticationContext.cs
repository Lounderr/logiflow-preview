namespace LogiFlowAPI.Services.Helpers
{
    using System;
    using System.Security.Claims;

    using LogiFlowAPI.Services.Interfaces;

    using Microsoft.AspNetCore.Http;

    public class AuthenticationContext : IAuthenticationContext
    {
        private readonly HttpContext httpContext;

        public AuthenticationContext(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContext = httpContextAccessor.HttpContext;
        }

        public ClaimsPrincipal User => this.httpContext.User;

        public string AuthToken => this.httpContext.Request.Headers["Authorization"].ToString()
            .Replace("Bearer ", string.Empty, StringComparison.OrdinalIgnoreCase).Trim();
    }
}
