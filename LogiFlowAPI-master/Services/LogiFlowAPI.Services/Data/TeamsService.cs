namespace LogiFlowAPI.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using LogiFlowAPI.Common.Extensions;
    using LogiFlowAPI.Data.Common.Repositories;
    using LogiFlowAPI.Data.Models;
    using LogiFlowAPI.Services.Common;
    using LogiFlowAPI.Services.Common.Result;
    using LogiFlowAPI.Services.Interfaces;
    using LogiFlowAPI.Services.Mapping;
    using LogiFlowAPI.Web.Models.Teams;
    using LogiFlowAPI.Web.Models.Warehouses;

    using Microsoft.EntityFrameworkCore;

    public class TeamsService : DataService, ITeamsService
    {
        private readonly IDeletableEntityRepository<Team> teamsRepo;
        private readonly IDeletableEntityRepository<User> usersRepo;
        private readonly string userId;

        public TeamsService(
            IDeletableEntityRepository<Team> teams,
            IAuthenticationContext authenticationContextService,
            IDeletableEntityRepository<User> usersRepo)
        {
            this.teamsRepo = teams;
            this.userId = authenticationContextService.User.Claims.GetId();
            this.usersRepo = usersRepo;
        }

        public async Task<Result<int>> CreateTeamAsync(CreateTeamModel model)
        {
            var team = this.Map<Team>(model);
            var user = await this.usersRepo.All().FirstAsync(x => x.Id == this.userId);

            user.OwnedTeams.Add(team);
            team.Owner = user;

            user.Teams.Add(team);
            team.Members.Add(user);

            await this.teamsRepo.AddAsync(team);
            await this.teamsRepo.SaveChangesAsync();

            return Result<int>.Ok(200, team.Id);
        }

        public async Task<Result> DeleteTeamAsync(int teamId)
        {
            var team = await this.teamsRepo.AllAsNoTracking().FirstOrDefaultAsync(t => t.Id == teamId && t.OwnerId == this.userId);

            if (team == null)
            {
                return Result.Error(404, ClientMessages.NotFoundOrInaccessible);
            }

            this.teamsRepo.Delete(team);
            await this.teamsRepo.SaveChangesAsync();

            return Result.Ok(200);
        }

        public async Task<Result<GetTeamModel>> GetTeamByIdAsync(int teamId)
        {
            var team = await this.teamsRepo
                .AllAsNoTracking()
                .Where(t => t.Id == teamId && t.Members.Any(m => m.Id == this.userId))
                .To<GetTeamModel>()
                .FirstOrDefaultAsync();

            if (team == null)
            {
                return Result<GetTeamModel>.Error(404, ClientMessages.NotFoundOrInaccessible);
            }

            return Result<GetTeamModel>.Ok(team);
        }

        public async Task<Result<GetAllTeamsModel>> GetAllTeamsAsync()
        {
            var teams = await this.teamsRepo
                .AllAsNoTracking()
                .Include(t => t.Owner)
                .Where(t => t.Members.Any(m => m.Id == this.userId))
                .ToListAsync();

            var ownedTeams = teams.Where(t => t.Owner.Id == this.userId);
            var joinedTeams = teams.Where(t => t.Owner.Id != this.userId);

            return Result<GetAllTeamsModel>.Ok(new GetAllTeamsModel()
            {
                OwnedTeams = this.MapTeams(ownedTeams),
                JoinedTeams = this.MapTeams(joinedTeams),
            });
        }

        public async Task<Result<IEnumerable<WarehouseSummaryModel>>> GetTeamWarehousesAsync(int teamId)
        {
            var team = await this.teamsRepo
                .AllAsNoTracking()
                .Where(t => t.Id == teamId && t.Members.Any(m => m.Id == this.userId))
                .Select(t => new { t.Warehouses })
                .FirstOrDefaultAsync();

            if (team == null)
            {
                return Result<IEnumerable<WarehouseSummaryModel>>.Error(404, ClientMessages.NotFoundOrInaccessible);
            }

            var warehouses = this.Map<IEnumerable<WarehouseSummaryModel>>(team.Warehouses);

            return Result<IEnumerable<WarehouseSummaryModel>>.Ok(warehouses);
        }

        private IEnumerable<TeamSummaryModel> MapTeams(IEnumerable<Team> teams) => this.Map<IEnumerable<TeamSummaryModel>>(teams);
    }
}
