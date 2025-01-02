namespace LogiFlowAPI.Web.Controllers
{
    using System.Threading.Tasks;

    using LogiFlowAPI.Services.Interfaces;
    using LogiFlowAPI.Web.Infrastructure.Extensions;
    using LogiFlowAPI.Web.Models.Teams;

    using Microsoft.AspNetCore.Mvc;

    [Route("api/teams")]
    public class TeamsController : ProtectedController
    {
        private readonly ITeamsService teamsService;

        public TeamsController(ITeamsService teamsService)
        {
            this.teamsService = teamsService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeamAsync(CreateTeamModel model)
        {
            return (await this.teamsService.CreateTeamAsync(model)).ToActionResult();
        }

        [HttpDelete("{teamId}")]
        public async Task<IActionResult> DeleteTeamAsync(int teamId)
        {
            return (await this.teamsService.DeleteTeamAsync(teamId)).ToActionResult();
        }

        [HttpGet("{teamId}")]
        public async Task<IActionResult> GetTeamByIdAsync(int teamId)
        {
            return (await this.teamsService.GetTeamByIdAsync(teamId)).ToActionResult();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTeamsAsync()
        {
            return (await this.teamsService.GetAllTeamsAsync()).ToActionResult();
        }

        [HttpGet("{teamId}/warehouses")]
        public async Task<IActionResult> GetTeamWarehousesAsync(int teamId)
        {
            return (await this.teamsService.GetTeamWarehousesAsync(teamId)).ToActionResult();
        }
    }
}
