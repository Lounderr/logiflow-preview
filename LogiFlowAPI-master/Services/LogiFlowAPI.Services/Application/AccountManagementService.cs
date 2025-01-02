namespace LogiFlowAPI.Services.Application
{
    using System.Linq;
    using System.Threading.Tasks;

    using LogiFlowAPI.Common.Extensions;
    using LogiFlowAPI.Data.Models;
    using LogiFlowAPI.Services.Common.Result;
    using LogiFlowAPI.Services.Interfaces;
    using LogiFlowAPI.Web.Models.Identity;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.IdentityModel.Tokens;

    public partial class AccountManagementService : IAccountManagementService
    {
        private readonly UserManager<User> userManager;
        private readonly ITokenService tokenService;
        private readonly string userId;

        public AccountManagementService(
            UserManager<User> userManager,
            ITokenService tokenService,
            IAuthenticationContext authenticationContext)
        {
            this.userManager = userManager;
            this.tokenService = tokenService;
            this.userId = authenticationContext.User.Claims.GetId();
        }

        public async Task<Result> ChangePasswordAsync(ChangePasswordModel model)
        {
            var userId = this.userId;

            var user = await this.userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return Result.Error(401, "User not found.");
            }

            var identityResult = await this.userManager.ChangePasswordAsync(
                user,
                model.OldPassword,
                model.NewPassword);

            var errorMessage = string.Join(" ", identityResult.Errors.Select(x => x.Description));

            if (identityResult.Succeeded)
            {
                await this.tokenService.RevokeAllRefreshTokens(user.Id);
                return Result.Ok();
            }

            return Result.Error(errorMessage);
        }

        public async Task<Result> UpdateDetailsAsync(UpdateDetailsModel model)
        {
            var userId = this.userId;

            var user = await this.userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return Result.Error(401, "User not found.");
            }

            // Update the account fields
            if (!model.UserName.IsNullOrEmpty())
            {
                user.UserName = model.UserName;
            }

            if (!model.Email.IsNullOrEmpty())
            {
                user.Email = model.Email;
            }

            if (!model.PhoneNumber.IsNullOrEmpty())
            {
                user.PhoneNumber = model.PhoneNumber;
            }

            var identityResult = await this.userManager.UpdateAsync(user);

            var errorMessage = string.Join(" ", identityResult.Errors.Select(x => x.Description));

            if (identityResult.Succeeded)
            {
                return Result.Ok();
            }

            return Result.Error(errorMessage);
        }
    }
}
