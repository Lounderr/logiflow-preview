namespace LogiFlowAPI.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class InviteStatus
    {
        [Key]
        public int Id { get; set; }

        public string Status { get; set; } = null!;
    }
}
