using Microsoft.EntityFrameworkCore;

namespace ETickets.Models
{
    [PrimaryKey(nameof(ApplicationUserId) , nameof(MovieId))]
    public class Cart
    {
        public string ApplicationUserId { get; set; } = null!;
        public int MovieId { get; set; }
        public int? CinemaId { get; set; }
        public Cinema? Cinema { get; set; }
        public Movie Movie { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
        public int Count{ get; set; }

    }
}
