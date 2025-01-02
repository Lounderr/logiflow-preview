namespace LogiFlowAPI.Web.Controllers
{
    using System.Threading.Tasks;

    using LogiFlowAPI.Services.Interfaces;
    using LogiFlowAPI.Web.Infrastructure.Extensions;
    using LogiFlowAPI.Web.Models.Deliveries;

    using Microsoft.AspNetCore.Mvc;

    [Route("api/deliveries")]
    public class DeliveriesController : ProtectedController
    {
        private readonly IDeliveriesService deliveriesService;

        public DeliveriesController(IDeliveriesService deliveriesService)
        {
            this.deliveriesService = deliveriesService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDeliveryAsync(CreateDeliveryModel model)
        {
            return (await this.deliveriesService.CreateDeliveryAsync(model)).ToActionResult();
        }

        [HttpPost("{deliveryId}/status")]
        public async Task<IActionResult> SetStatusAsync(SetDeliveryStatusModel model)
        {
            return (await this.deliveriesService.SetStatusAsync(model)).ToActionResult();
        }

        [HttpDelete("{deliveryId}")]
        public async Task<IActionResult> DeleteDeliveryAsync(int deliveryId)
        {
            return (await this.deliveriesService.DeleteDeliveryAsync(deliveryId)).ToActionResult();
        }

        [HttpGet("{deliveryId}")]
        public async Task<IActionResult> GetDeliveryByIdAsync(int deliveryId)
        {
            return (await this.deliveriesService.GetDeliveryByIdAsync(deliveryId)).ToActionResult();
        }

        [HttpGet("{deliveryId}/batches")]
        public async Task<IActionResult> GetDeliveryBatchesAsync([FromRoute] GetDeliveryBatchesModel model)
        {
            return (await this.deliveriesService.GetDeliveryBatchesAsync(model)).ToActionResult();
        }
    }
}
