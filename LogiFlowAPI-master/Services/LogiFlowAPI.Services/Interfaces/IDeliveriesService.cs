namespace LogiFlowAPI.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using LogiFlowAPI.Services.Common.Result;
    using LogiFlowAPI.Services.Interfaces.ServiceLifetimes;
    using LogiFlowAPI.Web.Models.Batches;
    using LogiFlowAPI.Web.Models.Deliveries;

    public interface IDeliveriesService : ITransientService
    {
        Task<Result<int>> CreateDeliveryAsync(CreateDeliveryModel model);
        Task<Result> DeleteDeliveryAsync(int deliveryId);
        Task<Result<IEnumerable<BatchSummaryModel>>> GetDeliveryBatchesAsync(GetDeliveryBatchesModel model);
        Task<Result<GetDeliveryModel>> GetDeliveryByIdAsync(int deliveryId);
        Task<Result> SetStatusAsync(SetDeliveryStatusModel model);
    }
}
