namespace ETickets.Models
{
    public class Chair
    {
        public int Id { get; set; }
        public int CinemaId { get; set; }
        public bool IsTaken { get; set; } = false;
        public Cinema Cinema { get; set; } = null!;
    }
}
