namespace LogiFlowAPI.Web.Models.Teams
{
    using LogiFlowAPI.Data.Models;
    using LogiFlowAPI.Services.Mapping;

    public class TeamSummaryModel : IMapFrom<Team>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
