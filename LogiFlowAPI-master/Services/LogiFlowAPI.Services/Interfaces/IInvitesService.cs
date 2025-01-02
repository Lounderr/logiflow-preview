namespace LogiFlowAPI.Services.Interfaces
{
    using System.Threading.Tasks;

    using LogiFlowAPI.Services.Common.Result;
    using LogiFlowAPI.Web.Models.Invites;

    public interface IInvitesService
    {
        Task<Result> AcceptInviteAsync(string inviteId);
        Task<Result> CancelInviteAsync(string inviteId);
        Task<Result<string>> CreateInviteAsync(CreateInviteModel model);
        Task<Result> DeclineInviteAsync(string inviteId);
    }
}