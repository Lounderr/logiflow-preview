namespace LogiFlowAPI.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using LogiFlowAPI.Services.Common.Result;
    using LogiFlowAPI.Services.Interfaces.ServiceLifetimes;
    using LogiFlowAPI.Web.Models.Deliveries;
    using LogiFlowAPI.Web.Models.StorageAreas;
    using LogiFlowAPI.Web.Models.Warehouses;

    public interface IWarehousesService : ITransientService
    {
        Task<Result<int>> CreateWarehouseAsync(CreateWarehouseModel model);
        Task<Result> DeleteWarehouseAsync(int warehouseId);
        Task<Result<IEnumerable<StorageAreaSummaryModel>>> GetWarehouseStorageAreasAsync(int warehouseId);
        Task<Result<GetWarehouseModel>> GetWarehouseByIdAsync(int warehouseId);
        Task<Result<IEnumerable<DeliverySummaryModel>>> GetWarehouseDeliveriesAsync(GetWarehouseDeliveriesModel model);
    }
}