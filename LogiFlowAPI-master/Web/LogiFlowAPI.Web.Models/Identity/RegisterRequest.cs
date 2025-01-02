namespace LogiFlowAPI.Web.Models.Identity
{
    using System.ComponentModel.DataAnnotations;

    public class RegisterRequest
    {
        [Required]
        [MaxLength(36)]
        public string Username { get; set; }

        [Required]
        [MaxLength(256)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Length(6, 256)]
        public string Password { get; set; }
    }
}
