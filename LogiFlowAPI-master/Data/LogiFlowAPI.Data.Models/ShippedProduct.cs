namespace LogiFlowAPI.Data.Models
{
    using System.Collections.Generic;

    using LogiFlowAPI.Data.Common.Models;

    public class ShippedProduct : BaseProduct
    {
        public ShippedProduct()
        {
            this.Tags = new HashSet<Tag>();
            //this.ShippedProductTags = new HashSet<ShippedProductTag>();
        }

        public int ShipmentId { get; set; }

        public virtual Shipment Shipment { get; set; } = null!;

        public virtual ICollection<Tag> Tags { get; set; }

        //public virtual ICollection<ShippedProductTag> ShippedProductTags { get; set; }
    }
}
