namespace LogiFlowAPI.Web.Controllers
{
    using System.Threading.Tasks;

    using LogiFlowAPI.Services.Interfaces;
    using LogiFlowAPI.Web.Infrastructure.Extensions;
    using LogiFlowAPI.Web.Models.Warehouses;

    using Microsoft.AspNetCore.Mvc;

    [Route("api/warehouses")]
    public class WarehousesController : ProtectedController
    {
        private readonly IWarehousesService warehousesService;

        public WarehousesController(IWarehousesService warehousesService)
        {
            this.warehousesService = warehousesService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateWarehouseAsync(CreateWarehouseModel model)
        {
            return (await this.warehousesService.CreateWarehouseAsync(model)).ToActionResult();
        }

        [HttpDelete("{warehouseId}")]
        public async Task<IActionResult> DeleteWarehouseAsync(int warehouseId)
        {
            return (await this.warehousesService.DeleteWarehouseAsync(warehouseId)).ToActionResult();
        }

        [HttpGet("{warehouseId}")]
        public async Task<IActionResult> GetWarehouseByIdAsync(int warehouseId)
        {
            return (await this.warehousesService.GetWarehouseByIdAsync(warehouseId)).ToActionResult();
        }

        [HttpGet("{warehouseId}/storage-areas")]
        public async Task<IActionResult> GetWarehouseAreasAsync(int warehouseId)
        {
            return (await this.warehousesService.GetWarehouseStorageAreasAsync(warehouseId)).ToActionResult();
        }

        [HttpGet("{warehouseId}/deliveries")]
        public async Task<IActionResult> GetWarehouseDeliveriesAsync(GetWarehouseDeliveriesModel model)
        {
            return (await this.warehousesService.GetWarehouseDeliveriesAsync(model)).ToActionResult();
        }
    }
}
