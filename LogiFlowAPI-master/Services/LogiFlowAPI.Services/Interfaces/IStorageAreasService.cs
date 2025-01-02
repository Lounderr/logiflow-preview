namespace LogiFlowAPI.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using LogiFlowAPI.Services.Common.Result;
    using LogiFlowAPI.Services.Interfaces.ServiceLifetimes;
    using LogiFlowAPI.Web.Models.Batches;
    using LogiFlowAPI.Web.Models.StorageAreas;

    public interface IStorageAreasService : ITransientService
    {
        Task<Result<int>> CreateStorageAreaAsync(CreateStorageAreaModel model);
        Task<Result> DeleteStorageAreaAsync(int storageAreaId);
        Task<Result<IEnumerable<BatchSummaryModel>>> GetStorageAreaBatchesAsync(GetStorageAreaBatchesModel model);
        Task<Result<GetStorageAreaModel>> GetStorageAreaByIdAsync(int storageAreaId);
    }
}
