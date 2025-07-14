namespace ETickets.Models
{
    public class Chair
    {
        public int Id { get; set; }

        public int CinemaId { get; set; } 
        public Cinema Cinema { get; set; } = null!;
    }
}
