namespace ETickets.Models
{
    public class ApplicationUserOTP
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public DateTime SendDate { get; set; }
        public int OTPNumber { get; set; }
        public DateTime ValidTo { get; set; }
        public bool Status{ get; set; }
        public string? Reason { get; set; }
        public ApplicationUser ApplicationUser { get; set; } = null!;

    }
}
