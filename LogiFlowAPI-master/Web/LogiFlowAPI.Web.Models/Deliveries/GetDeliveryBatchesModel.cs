namespace LogiFlowAPI.Web.Models.Deliveries
{
    using System.ComponentModel.DataAnnotations;

    public class GetDeliveryBatchesModel
    {
        [Required]
        public int DeliveryId { get; set; }

        [Range(1, int.MaxValue)]
        public int Page { get; set; }
    }
}
