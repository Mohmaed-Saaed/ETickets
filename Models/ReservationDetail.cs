using Microsoft.EntityFrameworkCore;

namespace ETickets.Models
{
    [PrimaryKey(nameof(ReservationId),nameof(MovieId))]
    public class ReservationDetail
    {
        public int ReservationId { get; set; } 
        public int MovieId { get; set; }
        public int? ChairId { get; set; }
        public int? CinemaId { get; set; }
        public decimal MoviePrice { get; set; }
        public DateTime? MovieDisplayTime { get; set; }
        public Reservation Reservation { get; set; } = null!;
        public Cinema Cinema { get; set; } = null!;
        public Movie Movie { get; set; } = null!;
        public Chair Chair { get; set; } = null!;
    }
}
