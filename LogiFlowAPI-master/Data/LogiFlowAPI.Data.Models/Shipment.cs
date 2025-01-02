namespace LogiFlowAPI.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using LogiFlowAPI.Data.Common;
    using LogiFlowAPI.Data.Common.Models;

    public class Shipment : IdentifiableModel<int>
    {
        public Shipment()
        {
            this.ShippedProducts = new HashSet<ShippedProduct>();
        }

        [StringLength(ModelConstants.Shipment.RecipientMaxLength)]
        public string Recipient { get; set; } = null!;

        [StringLength(ModelConstants.Shipment.ShippingAddress)]
        public string ShippingAddress { get; set; } = null!;

        public int WarehouseId { get; set; }

        public virtual Warehouse Warehouse { get; set; } = null!;

        public virtual ICollection<ShippedProduct> ShippedProducts { get; set; }
    }
}
