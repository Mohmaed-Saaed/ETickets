using ETickets.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETickets.ModelView
{
    public class MovieDaysVM
    {

        public int MovieDayId { get; set; }
        public int MovieId { get; set; }
        [Display(Name = "Movie name")]
        public string? MovieName{ get; set; }

        public string? DayName{ get; set; }

        public DateOnly Date { get; set; } 
        public TimeOnly From { get; set; }
        public TimeOnly To { get; set; }

        public TimeSpan? Duration { get; set; }

        [Display(Name = "Movie start date")]
        public DateOnly MovieStartDate { get; set; }
        [Display(Name = "Movie end date")]
        public DateOnly MovieEndDate { get; set; }

        public int AvailableDays { get; set; }
        [Display(Name =  "Additional time after movie finishes ( Minutes )")]
        public int MinutesRestAfterMovie { get; set; }
        public Movie Movie { get; set; } = null!;

        [Required]
        [Display(Name = "Day")]
        public int? DayId { get; set; } = null!;

 
        public Day Day { get; set; } = null!;


        //public int MovieDayId { get; set; }
        public IEnumerable<SelectListItem> Days { get; set; } = new List<SelectListItem>();
        public IEnumerable<DateTime> dateTimes { get; set; } = new List<DateTime>();
    }
}
