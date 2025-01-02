namespace LogiFlowAPI.Services.Interfaces
{
    using System.Threading.Tasks;

    using LogiFlowAPI.Services.Common.Result;
    using LogiFlowAPI.Services.Interfaces.ServiceLifetimes;
    using LogiFlowAPI.Web.Models.Identity;

    public interface IAccountManagementService : IScopedService
    {
        Task<Result> ChangePasswordAsync(ChangePasswordModel model);

        Task<Result> UpdateDetailsAsync(UpdateDetailsModel model);
    }
}