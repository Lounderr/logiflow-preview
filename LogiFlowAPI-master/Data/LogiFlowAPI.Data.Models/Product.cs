namespace LogiFlowAPI.Data.Models
{
    using System.Collections.Generic;

    using LogiFlowAPI.Data.Common.Models;

    public class Product : BaseProduct
    {
        public Product()
        {
            this.Batches = new HashSet<Batch>();
            this.Tags = new HashSet<Tag>();
            //this.ProductTags = new HashSet<ProductTag>();
        }

        public int WarehouseId { get; set; }

        public virtual Warehouse Warehouse { get; set; } = null!;

        public virtual ICollection<Batch> Batches { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }

        //public virtual ICollection<ProductTag> ProductTags { get; set; }
    }
}
