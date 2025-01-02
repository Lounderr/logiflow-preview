namespace LogiFlowAPI.Web.Models.Warehouses
{
    using System.ComponentModel.DataAnnotations;

    using LogiFlowAPI.Data.Models;
    using LogiFlowAPI.Services.Mapping;

    public class CreateWarehouseModel : IMapTo<Batch>
    {
        [Required]
        public int TeamId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }
    }
}
