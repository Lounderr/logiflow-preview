namespace LogiFlowAPI.Services.Application
{
    using System.Linq;
    using System.Threading.Tasks;

    using LogiFlowAPI.Common.Extensions;
    using LogiFlowAPI.Data.Models;
    using LogiFlowAPI.Services.Common;
    using LogiFlowAPI.Services.Common.Result;
    using LogiFlowAPI.Services.Interfaces;
    using LogiFlowAPI.Web.Models.Auth;
    using LogiFlowAPI.Web.Models.Identity;

    using Microsoft.AspNetCore.Identity;

    public partial class AuthService : IAuthService
    {
        private readonly UserManager<User> userManager;
        private readonly ITokenService tokenService;

        public AuthService(
            UserManager<User> userManager,
            ITokenService tokenService)
        {
            this.userManager = userManager;
            this.tokenService = tokenService;
        }

        public async Task<Result<TokenPairModel>> LoginAsync(LoginRequest request)
        {
            var user = await this.userManager.FindByNameAsync(request.UserName);

            if (user == null)
            {
                return Result<TokenPairModel>.Error(401, ClientMessages.Identity.InvalidCredentials);
            }

            var passwordValid = await this.userManager.CheckPasswordAsync(user, request.Password);

            if (!passwordValid)
            {
                return Result<TokenPairModel>.Error(401, ClientMessages.Identity.InvalidCredentials);
            }

            return Result<TokenPairModel>.Ok(new TokenPairModel
            {
                RefreshToken = await this.tokenService.GenerateRefreshToken(user.Id),
                AccessToken = await this.tokenService.GenerateAuthToken(user)
            });
        }

        public async Task<Result> RegisterAsync(RegisterRequest request)
        {
            User user = new()
            {
                UserName = request.Username,
                Email = request.Email,
            };

            var identityResult = await this.userManager.CreateAsync(user, request.Password);

            var errorMessage = string.Join(" ", identityResult.Errors.Select(x => x.Description));

            return identityResult.Succeeded ? Result.Ok() : Result.Error(errorMessage);
        }

        public async Task<Result<TokenPairModel>> RefreshToken(string refreshToken)
        {
            if (!await this.tokenService.ValidateRefreshToken(refreshToken))
            {
                return Result<TokenPairModel>.Error("Invalid refresh token.");
            }

            var userId = this.tokenService.ExtractClaims(refreshToken).GetId();

            var user = await this.userManager.FindByIdAsync(userId);

            return Result<TokenPairModel>.Ok(new TokenPairModel
            {
                RefreshToken = await this.tokenService.GenerateRefreshToken(userId),
                AccessToken = await this.tokenService.GenerateAuthToken(user)
            });
        }
    }
}
