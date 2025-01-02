namespace LogiFlowAPI.Data.Models
{
    using LogiFlowAPI.Data.Common.Models;

    using Microsoft.EntityFrameworkCore;

    [PrimaryKey(nameof(StorageAreaId), nameof(TagId))]
    public class StorageAreaTag : BaseDeletableModel
    {
        public int StorageAreaId { get; set; }

        public virtual StorageArea StorageArea { get; set; } = null!;

        public int TagId { get; set; }

        public virtual Tag Tag { get; set; } = null!;
    }
}
