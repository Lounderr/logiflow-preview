namespace LogiFlowAPI.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using LogiFlowAPI.Data.Models;
    using LogiFlowAPI.Services.Interfaces.ServiceLifetimes;

    public interface ITokenService : IScopedService
    {
        IEnumerable<Claim> ExtractClaims(string token);
        Task<string> GenerateAuthToken(User user);
        Task<string> GenerateRefreshToken(string userId);
        Task RevokeAllRefreshTokens(string userId);
        Task RevokeToken(string token);
        Task<bool> ValidateRefreshToken(string token);
    }
}
