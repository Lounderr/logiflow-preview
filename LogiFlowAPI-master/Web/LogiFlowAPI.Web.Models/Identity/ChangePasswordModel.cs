namespace LogiFlowAPI.Web.Models.Identity
{
    using System.ComponentModel.DataAnnotations;

    public class ChangePasswordModel
    {
        [Required]
        public string OldPassword { get; set; }

        [Required]
        [Length(6, 256)]
        public string NewPassword { get; set; }
    }
}
