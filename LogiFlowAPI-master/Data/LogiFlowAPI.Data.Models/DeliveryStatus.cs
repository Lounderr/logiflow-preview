namespace LogiFlowAPI.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using LogiFlowAPI.Data.Common;

    public class DeliveryStatus
    {
        public DeliveryStatus()
        {
            this.Deliveries = new HashSet<Delivery>();
        }

        [Key]
        public int Id { get; set; }

        [StringLength(ModelConstants.DeliveryStatus.StatusMaxLength)]
        public string Status { get; set; } = null!;

        public virtual ICollection<Delivery> Deliveries { get; set; }
    }
}
