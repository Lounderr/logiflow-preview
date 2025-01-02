namespace LogiFlowAPI.Web.Models.Warehouses
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class GetWarehouseDeliveriesModel
    {
        [Required]
        public int WarehouseId { get; set; }

        [Range(1, int.MaxValue)]
        public int Page { get; set; }
    }
}
