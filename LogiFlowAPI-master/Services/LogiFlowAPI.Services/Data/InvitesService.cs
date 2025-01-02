namespace LogiFlowAPI.Services.Data
{
    using System.Threading.Tasks;

    using LogiFlowAPI.Common.Extensions;
    using LogiFlowAPI.Data.Common.Repositories;
    using LogiFlowAPI.Data.Models;
    using LogiFlowAPI.Data.Models.Enums;
    using LogiFlowAPI.Services.Common;
    using LogiFlowAPI.Services.Common.Result;
    using LogiFlowAPI.Services.Interfaces;
    using LogiFlowAPI.Web.Models.Invites;

    using Microsoft.EntityFrameworkCore;

    public class InvitesService : DataService, IInvitesService
    {
        private readonly IRepository<Invite> invitesRepo;
        private readonly IRepository<Team> teamRepo;
        private readonly IRepository<User> userRepo;
        private readonly string userId;

        public InvitesService(
            IRepository<Invite> invitesRepo,
            IRepository<Team> teamRepo,
            IRepository<User> userRepo,
            IAuthenticationContext authenticationContextService)
        {
            this.invitesRepo = invitesRepo;
            this.teamRepo = teamRepo;
            this.userRepo = userRepo;
            this.userId = authenticationContextService.User.Claims.GetId();
        }

        public async Task<Result<string>> CreateInviteAsync(CreateInviteModel model)
        {
            var isTeamAccessible = await this.teamRepo.AnyAsync(t => t.Id == model.TeamId && t.OwnerId == this.userId);

            if (!isTeamAccessible)
            {
                return Result<string>.Error(404, ClientMessages.NotFoundOrInaccessible);
            }

            var recipient = await this.userRepo.AllAsNoTracking().FirstOrDefaultAsync(u => u.UserName == model.UserName);

            if (recipient == null)
            {
                return Result<string>.Error(404, ClientMessages.NotFoundOrInaccessible);
            }

            var inviteAlreadyExists = await this.invitesRepo.AnyAsync(i =>
                i.RecipientId == recipient.Id &&
                i.TeamId == model.TeamId && i.InviteStatusId == (int)InviteStatusOptions.Pending);

            if (inviteAlreadyExists)
            {
                return Result<string>.Error(400, "User has already been invited.");
            }

            Invite invite = new()
            {
                RecipientId = recipient.Id,
                TeamId = model.TeamId,
                InviteStatusId = (int)InviteStatusOptions.Pending,
            };

            await this.invitesRepo.AddAsync(invite);
            await this.invitesRepo.SaveChangesAsync();
            return Result<string>.Ok(invite.Id);
        }

        public async Task<Result> DeclineInviteAsync(string inviteId)
        {
            var invite = await this.invitesRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == inviteId && i.RecipientId == this.userId);

            if (invite == null)
            {
                return Result.Error(404, ClientMessages.NotFoundOrInaccessible);
            }

            invite.InviteStatusId = (int)InviteStatusOptions.Declined;
            await this.invitesRepo.SaveChangesAsync();
            return Result.Ok();
        }

        public async Task<Result> AcceptInviteAsync(string inviteId)
        {
            var invite = await this.invitesRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == inviteId && i.RecipientId == this.userId);

            if (invite == null)
            {
                return Result.Error(404, ClientMessages.NotFoundOrInaccessible);
            }

            var recipient = await this.userRepo.All().FirstOrDefaultAsync(u => u.Id == invite.RecipientId);
            recipient.Teams.Add(new Team { Id = invite.TeamId });

            var team = await this.teamRepo.All().FirstOrDefaultAsync(t => t.Id == invite.TeamId);

            recipient.Teams.Add(team);
            team.Members.Add(recipient);

            invite.InviteStatusId = (int)InviteStatusOptions.Accepted;

            await this.userRepo.SaveChangesAsync();
            await this.teamRepo.SaveChangesAsync();
            await this.invitesRepo.SaveChangesAsync();

            return Result.Ok();
        }

        public async Task<Result> CancelInviteAsync(string inviteId)
        {
            var invite = await this.invitesRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == inviteId && i.Team.OwnerId == this.userId);

            if (invite == null)
            {
                return Result.Error(404, ClientMessages.NotFoundOrInaccessible);
            }

            invite.InviteStatusId = (int)InviteStatusOptions.Canceled;
            await this.invitesRepo.SaveChangesAsync();
            return Result.Ok();
        }
    }
}
