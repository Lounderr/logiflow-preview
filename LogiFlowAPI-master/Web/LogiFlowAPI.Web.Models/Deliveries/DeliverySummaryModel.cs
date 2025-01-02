namespace LogiFlowAPI.Web.Models.Deliveries
{
    using LogiFlowAPI.Data.Models;
    using LogiFlowAPI.Services.Mapping;

    public class DeliverySummaryModel : IMapFrom<Delivery>
    {
        public int Id { get; set; }

        public string Sender { get; set; }
    }
}
