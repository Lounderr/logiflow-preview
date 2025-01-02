namespace LogiFlowAPI.Data.Models
{
    using LogiFlowAPI.Data.Common.Models;

    using Microsoft.EntityFrameworkCore;

    [PrimaryKey(nameof(ProductId), nameof(TagId))]
    public class ProductTag : BaseDeletableModel
    {
        public int ProductId { get; set; }

        public virtual Product Product { get; set; } = null!;

        public int TagId { get; set; }

        public virtual Tag Tag { get; set; } = null!;
    }
}
