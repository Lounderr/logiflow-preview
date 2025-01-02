namespace LogiFlowAPI.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using LogiFlowAPI.Data.Common;
    using LogiFlowAPI.Data.Common.Models;

    public class Warehouse : IdentifiableModel<int>
    {
        public Warehouse()
        {
            this.Shipments = new HashSet<Shipment>();
            this.Deliveries = new HashSet<Delivery>();
            this.StorageAreas = new HashSet<StorageArea>();
        }

        [StringLength(ModelConstants.Warehouse.NameMaxLength)]
        public string Name { get; set; } = null!;

        public int TeamId { get; set; }

        public virtual Team Team { get; set; } = null!;

        public virtual ICollection<Shipment> Shipments { get; set; }

        public virtual ICollection<Delivery> Deliveries { get; set; }

        public virtual ICollection<StorageArea> StorageAreas { get; set; }
    }
}
