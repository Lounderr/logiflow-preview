namespace LogiFlowAPI.Web.Models.Identity
{
    using System.ComponentModel.DataAnnotations;

    public class UpdateDetailsModel
    {
        [Required]
        [MaxLength(36)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(256)]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }
    }
}
