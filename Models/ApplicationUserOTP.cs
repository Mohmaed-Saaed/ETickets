using System.ComponentModel.DataAnnotations.Schema;

namespace ETickets.Models
{
    public class ApplicationUserOTP
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; } = null!;
        public DateTime SendDate { get; set; }
        public int OTPNumber { get; set; }
        public DateTime ValidTo { get; set; }
        public bool Status{ get; set; }
        public string? Reason { get; set; }
        public ApplicationUser ApplicationUser { get; set; } = null!;

    }
}
