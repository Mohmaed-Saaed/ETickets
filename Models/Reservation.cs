using static ETickets.Global;

namespace ETickets.Models
{
  
    public class Reservation
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public decimal Total { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string? PaymentId { get; set; }
        public bool IsPaid { get; set; }
        public string? SessionId { get; set; }
        public ApplicationUser User { get; set; } = null!;
    }
}