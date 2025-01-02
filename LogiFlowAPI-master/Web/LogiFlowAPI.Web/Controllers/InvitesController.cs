namespace LogiFlowAPI.Web.Controllers
{
    using System.Threading.Tasks;

    using LogiFlowAPI.Services.Interfaces;
    using LogiFlowAPI.Web.Infrastructure.Extensions;
    using LogiFlowAPI.Web.Models.Invites;

    using Microsoft.AspNetCore.Mvc;

    [Route("api/invites")]
    public class InvitesController : ProtectedController
    {
        private readonly IInvitesService invitesService;

        public InvitesController(IInvitesService invitesService)
        {
            this.invitesService = invitesService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBatchAsync(CreateInviteModel model)
        {
            return (await this.invitesService.CreateInviteAsync(model)).ToActionResult();
        }

        [HttpPost("{inviteId}/accept")]
        public async Task<IActionResult> AcceptInviteAsync(string inviteId)
        {
            return (await this.invitesService.AcceptInviteAsync(inviteId)).ToActionResult();
        }

        [HttpPost("{inviteId}/decline")]
        public async Task<IActionResult> DeclineInviteAsync(string inviteId)
        {
            return (await this.invitesService.DeclineInviteAsync(inviteId)).ToActionResult();
        }

        [HttpPost("{inviteId}/cancel")]
        public async Task<IActionResult> CancelInviteAsync(string inviteId)
        {
            return (await this.invitesService.CancelInviteAsync(inviteId)).ToActionResult();
        }
    }
}
