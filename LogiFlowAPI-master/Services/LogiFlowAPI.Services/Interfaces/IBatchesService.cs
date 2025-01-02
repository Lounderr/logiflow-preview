namespace LogiFlowAPI.Services.Interfaces
{
    using System.Threading.Tasks;

    using LogiFlowAPI.Services.Common.Result;
    using LogiFlowAPI.Web.Models.Batches;

    public interface IBatchesService
    {
        Task<Result<int>> CreateBatchAsync(CreateBatchModel model);
        Task<Result> DeleteBatchAsync(int batchId);
        Task<Result<GetBatchModel>> GetBatchByIdAsync(int batchId);
    }
}