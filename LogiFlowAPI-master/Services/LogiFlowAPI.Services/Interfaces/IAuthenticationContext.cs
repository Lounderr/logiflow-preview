namespace LogiFlowAPI.Services.Interfaces
{
    using System.Security.Claims;

    using LogiFlowAPI.Services.Interfaces.ServiceLifetimes;

    public interface IAuthenticationContext : IScopedService
    {
        ClaimsPrincipal User { get; }

        string AuthToken { get; }
    }
}
