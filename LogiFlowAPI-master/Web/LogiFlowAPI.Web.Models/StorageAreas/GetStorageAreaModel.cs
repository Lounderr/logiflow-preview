namespace LogiFlowAPI.Web.Models.StorageAreas
{
    using LogiFlowAPI.Data.Models;
    using LogiFlowAPI.Services.Mapping;

    public class GetStorageAreaModel : IMapFrom<StorageArea>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
