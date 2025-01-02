namespace LogiFlowAPI.Web.Models.Batches
{
    using LogiFlowAPI.Data.Models;
    using LogiFlowAPI.Services.Mapping;

    public class BatchSummaryModel : IMapFrom<Batch>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
