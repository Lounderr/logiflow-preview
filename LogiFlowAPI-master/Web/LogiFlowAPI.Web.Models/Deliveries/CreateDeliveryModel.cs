namespace LogiFlowAPI.Web.Models.Deliveries
{
    using LogiFlowAPI.Data.Models;
    using LogiFlowAPI.Services.Mapping;

    public class CreateDeliveryModel : IMapTo<Delivery>
    {
        public int WarehouseId { get; set; }

        public string Sender { get; set; }
    }
}
