namespace ETickets.Models
{
    public class MovieDay
    {
        public int Id { get; set; }
        public int MovieId{ get; set; }
        public int DayId{ get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly From { get; set; }
        public TimeOnly To { get; set; }
        public int MinutesRestAfterMovie { get; set; } 
        public Movie Movie { get; set; } = null!;
        public Day Day{ get; set; } = null!;
    }
}
