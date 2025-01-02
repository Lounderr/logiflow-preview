namespace LogiFlowAPI.Web.Models.StorageAreas
{
    using LogiFlowAPI.Data.Models;
    using LogiFlowAPI.Services.Mapping;

    public class CreateStorageAreaModel : IMapTo<StorageArea>
    {
        public int WarehouseId { get; set; }

        public string Name { get; set; }
    }
}
