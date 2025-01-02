namespace LogiFlowAPI.Data.Models
{
    using System;

    using LogiFlowAPI.Data.Common.Models;

    public class Invite : IdentifiableModel<string>
    {
        public Invite()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string RecipientId { get; set; } = null!;

        public virtual User Recipient { get; set; } = null!;

        public int TeamId { get; set; }

        public virtual Team Team { get; set; } = null!;

        public int InviteStatusId { get; set; }

        public virtual InviteStatus InviteStatus { get; set; } = null!;
    }
}
