namespace LogiFlowAPI.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using LogiFlowAPI.Data.Common;
    using LogiFlowAPI.Data.Common.Models;

    public class Team : IdentifiableModel<int>
    {
        public Team()
        {
            this.Tags = new HashSet<Tag>();
            this.Warehouses = new HashSet<Warehouse>();
            this.Invites = new HashSet<Invite>();
            this.Members = new HashSet<User>();
            //this.TeamMembers = new HashSet<TeamMember>();
        }

        [StringLength(ModelConstants.Team.NameMaxLength)]
        public string Name { get; set; } = null!;

        public string OwnerId { get; set; } = null!;

        [InverseProperty(nameof(User.OwnedTeams))]
        public virtual User Owner { get; set; } = null!;

        public virtual ICollection<Tag> Tags { get; set; }

        public virtual ICollection<Warehouse> Warehouses { get; set; }

        public virtual ICollection<Invite> Invites { get; set; }

        public virtual ICollection<User> Members { get; set; }

        //public virtual ICollection<TeamMember> TeamMembers { get; set; }
    }
}
