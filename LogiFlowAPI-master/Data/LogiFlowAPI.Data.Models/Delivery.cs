namespace LogiFlowAPI.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using LogiFlowAPI.Data.Common;
    using LogiFlowAPI.Data.Common.Models;

    public class Delivery : IdentifiableModel<int>
    {
        public Delivery()
        {
            this.Batches = new HashSet<Batch>();
        }

        [StringLength(ModelConstants.Delivery.SenderMaxLength)]
        public string Sender { get; set; } = null!;

        public int DeliveryStatusId { get; set; }

        public virtual DeliveryStatus DeliveryStatus { get; set; } = null!;

        public int WarehouseId { get; set; }

        public virtual Warehouse Warehouse { get; set; } = null!;

        public virtual ICollection<Batch> Batches { get; set; }
    }
}
