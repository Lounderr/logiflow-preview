namespace LogiFlowAPI.Data.Models
{
    using LogiFlowAPI.Data.Common.Models;

    using Microsoft.EntityFrameworkCore;

    [PrimaryKey(nameof(ShippedProductId), nameof(TagId))]
    public class ShippedProductTag : BaseDeletableModel
    {
        public int ShippedProductId { get; set; }

        public virtual ShippedProduct ShippedProduct { get; set; } = null!;

        public int TagId { get; set; }

        public virtual Tag Tag { get; set; } = null!;
    }
}
