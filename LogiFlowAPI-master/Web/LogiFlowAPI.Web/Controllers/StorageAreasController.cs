namespace LogiFlowAPI.Web.Controllers
{
    using System.Threading.Tasks;

    using LogiFlowAPI.Services.Interfaces;
    using LogiFlowAPI.Web.Infrastructure.Extensions;
    using LogiFlowAPI.Web.Models.StorageAreas;

    using Microsoft.AspNetCore.Mvc;

    [Route("api/storage-areas")]
    public class StorageAreasController : ProtectedController
    {
        private readonly IStorageAreasService storageAreasService;

        public StorageAreasController(IStorageAreasService storageAreasService)
        {
            this.storageAreasService = storageAreasService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStorageAreaAsync(CreateStorageAreaModel model)
        {
            return (await this.storageAreasService.CreateStorageAreaAsync(model)).ToActionResult();
        }

        [HttpDelete("{storageAreaId}")]
        public async Task<IActionResult> DeleteStorageAreaAsync(int storageAreaId)
        {
            return (await this.storageAreasService.DeleteStorageAreaAsync(storageAreaId)).ToActionResult();
        }

        [HttpGet("{storageAreaId}")]
        public async Task<IActionResult> GetWarehouseByIdAsync(int storageAreaId)
        {
            return (await this.storageAreasService.GetStorageAreaByIdAsync(storageAreaId)).ToActionResult();
        }

        [HttpGet("{storageAreaId}/batches")]
        public async Task<IActionResult> GetStorageAreaBatches([FromRoute] GetStorageAreaBatchesModel model)
        {
            return (await this.storageAreasService.GetStorageAreaBatchesAsync(model)).ToActionResult();
        }
    }
}
