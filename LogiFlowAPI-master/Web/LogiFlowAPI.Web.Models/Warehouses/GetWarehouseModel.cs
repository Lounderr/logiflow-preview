﻿namespace LogiFlowAPI.Web.Models.Warehouses
{
    using LogiFlowAPI.Data.Models;
    using LogiFlowAPI.Services.Mapping;

    public class GetWarehouseModel : IMapFrom<Batch>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
