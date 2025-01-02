namespace LogiFlowAPI.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using LogiFlowAPI.Data.Common;
    using LogiFlowAPI.Data.Common.Models;

    public class Tag : IdentifiableModel<int>
    {
        public Tag()
        {
            this.Products = new HashSet<Product>();
            //this.ProductTags = new HashSet<ProductTag>();
            this.StorageAreas = new HashSet<StorageArea>();
            //this.StorageAreaTags = new HashSet<StorageAreaTag>();
            this.ShippedProducts = new HashSet<ShippedProduct>();
            //this.ShippedProductTags = new HashSet<ShippedProductTag>();
        }

        [StringLength(ModelConstants.Tag.NameMaxLength)]
        public string Name { get; set; } = null!;

        public int TeamId { get; set; }

        public virtual Team Team { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; }

        //public virtual ICollection<ProductTag> ProductTags { get; set; }

        public virtual ICollection<StorageArea> StorageAreas { get; set; }

        //public virtual ICollection<StorageAreaTag> StorageAreaTags { get; set; }

        public virtual ICollection<ShippedProduct> ShippedProducts { get; set; }

        //public virtual ICollection<ShippedProductTag> ShippedProductTags { get; set; }
    }
}
