using ETickets.Models;

namespace ETickets.ModelView
{
    public class MovieDaysVM
    {
        public int MovieId { get; set; }
        public string? MovieName{ get; set; }
        public int DayId { get; set; }
        public string? DayName{ get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public int AvailableDays { get; set; } 
        public int MinutesRestAfterMovie { get; set; }
        public Movie Movie { get; set; } = null!;
        public Day Day { get; set; } = null!;

        public IEnumerable<DateTime> dateTimes { get; set; } = new List<DateTime>();
    }
}
