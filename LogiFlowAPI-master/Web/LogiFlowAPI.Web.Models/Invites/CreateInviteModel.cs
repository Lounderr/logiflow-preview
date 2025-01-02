namespace LogiFlowAPI.Web.Models.Invites
{
    using System.ComponentModel.DataAnnotations;

    public class CreateInviteModel
    {
        public int TeamId { get; set; }

        [EmailAddress]
        public string UserName { get; set; }
    }
}
