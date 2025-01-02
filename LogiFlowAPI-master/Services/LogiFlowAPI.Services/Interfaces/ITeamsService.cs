namespace LogiFlowAPI.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using LogiFlowAPI.Services.Common.Result;
    using LogiFlowAPI.Services.Interfaces.ServiceLifetimes;
    using LogiFlowAPI.Web.Models.Teams;
    using LogiFlowAPI.Web.Models.Warehouses;

    public interface ITeamsService : ITransientService
    {
        Task<Result<int>> CreateTeamAsync(CreateTeamModel model);
        Task<Result> DeleteTeamAsync(int teamId);
        Task<Result<GetAllTeamsModel>> GetAllTeamsAsync();
        Task<Result<GetTeamModel>> GetTeamByIdAsync(int teamId);
        Task<Result<IEnumerable<WarehouseSummaryModel>>> GetTeamWarehousesAsync(int teamId);
    }
}
