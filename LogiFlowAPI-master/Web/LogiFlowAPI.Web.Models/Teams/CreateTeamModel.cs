namespace LogiFlowAPI.Web.Models.Teams
{
    using System.ComponentModel.DataAnnotations;

    using LogiFlowAPI.Data.Models;
    using LogiFlowAPI.Services.Mapping;

    public class CreateTeamModel : IMapTo<Team>
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }
    }
}
