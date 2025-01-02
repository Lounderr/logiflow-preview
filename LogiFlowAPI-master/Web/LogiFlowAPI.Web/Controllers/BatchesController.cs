namespace LogiFlowAPI.Web.Controllers
{
    using System.Threading.Tasks;

    using LogiFlowAPI.Services.Interfaces;
    using LogiFlowAPI.Web.Infrastructure.Extensions;
    using LogiFlowAPI.Web.Models.Batches;

    using Microsoft.AspNetCore.Mvc;

    [Route("api/batches")]
    public class BatchesController : ProtectedController
    {
        private readonly IBatchesService batchesService;

        public BatchesController(IBatchesService batchesService)
        {
            this.batchesService = batchesService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBatchAsync(CreateBatchModel model)
        {
            return (await this.batchesService.CreateBatchAsync(model)).ToActionResult();
        }

        [HttpDelete("{batchId}")]
        public async Task<IActionResult> DeleteDeliveryAsync(int batchId)
        {
            return (await this.batchesService.DeleteBatchAsync(batchId)).ToActionResult();
        }

        [HttpGet("{batchId}")]
        public async Task<IActionResult> GetDeliveryByIdAsync(int batchId)
        {
            return (await this.batchesService.GetBatchByIdAsync(batchId)).ToActionResult();
        }
    }
}
