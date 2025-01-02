namespace LogiFlowAPI.Web.Models.Batches
{
    public class CreateBatchModel
    {
        public int DeliveryId { get; set; }

        public int StorageAreaId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
