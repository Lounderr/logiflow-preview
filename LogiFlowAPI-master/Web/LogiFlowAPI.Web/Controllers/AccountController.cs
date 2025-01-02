namespace LogiFlowAPI.Web.Controllers
{
    using System.Threading.Tasks;

    using LogiFlowAPI.Services.Interfaces;
    using LogiFlowAPI.Web.Infrastructure.Extensions;
    using LogiFlowAPI.Web.Models.Identity;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountManagementService accountService;

        public AccountController(IAccountManagementService accountService)
        {
            this.accountService = accountService;
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel request)
        {
            return (await this.accountService.ChangePasswordAsync(request)).ToActionResult();
        }

        [HttpPost("update-details")]
        public async Task<IActionResult> UpdateDetails(UpdateDetailsModel request)
        {
            return (await this.accountService.UpdateDetailsAsync(request)).ToActionResult();
        }
    }
}
