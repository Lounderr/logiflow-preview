namespace LogiFlowAPI.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using LogiFlowAPI.Data.Common.Models;

    public class Batch : IdentifiableModel<int>
    {
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public int DeliveryId { get; set; }

        public virtual Delivery Delivery { get; set; } = null!;

        public int ProductId { get; set; }

        public virtual Product Product { get; set; } = null!;

        public int StorageAreaId { get; set; }

        public virtual StorageArea StorageArea { get; set; } = null!;
    }
}
