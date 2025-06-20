namespace ETickets.Models
{
    public class MovieDisplayState
    {
        public int Id { get; set; }
        public string Status { get; set; } = null!;
        public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();

    }
}
