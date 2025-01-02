namespace LogiFlowAPI.Web.Models.Teams
{
    using System.Collections.Generic;

    public class GetAllTeamsModel
    {
        public IEnumerable<TeamSummaryModel> OwnedTeams { get; set; }

        public IEnumerable<TeamSummaryModel> JoinedTeams { get; set; }
    }
}
