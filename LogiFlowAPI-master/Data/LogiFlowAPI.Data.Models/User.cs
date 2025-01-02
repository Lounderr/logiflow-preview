namespace LogiFlowAPI.Data.Models
{
    using System;
    using System.Collections.Generic;

    using LogiFlowAPI.Data.Common.Models;

    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public User()
        {
            this.Id = Guid.NewGuid().ToString();
            this.OwnedTeams = new HashSet<Team>();
            this.Teams = new HashSet<Team>();
            //this.TeamMembers = new HashSet<TeamMember>();
            this.Invites = new HashSet<Invite>();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
        }

        public virtual ICollection<Team> OwnedTeams { get; set; }

        public virtual ICollection<Team> Teams { get; set; }

        //public virtual ICollection<TeamMember> TeamMembers { get; set; }

        public virtual ICollection<Invite> Invites { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
