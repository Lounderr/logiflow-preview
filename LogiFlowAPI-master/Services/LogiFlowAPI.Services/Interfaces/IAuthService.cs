namespace LogiFlowAPI.Services.Interfaces
{
    using System.Threading.Tasks;

    using LogiFlowAPI.Services.Common.Result;
    using LogiFlowAPI.Services.Interfaces.ServiceLifetimes;
    using LogiFlowAPI.Web.Models.Auth;
    using LogiFlowAPI.Web.Models.Identity;

    public interface IAuthService : IScopedService
    {
        Task<Result<TokenPairModel>> LoginAsync(LoginRequest request);
        Task<Result<TokenPairModel>> RefreshToken(string refreshToken);
        Task<Result> RegisterAsync(RegisterRequest request);
    }
}
