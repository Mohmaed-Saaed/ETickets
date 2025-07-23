using ETickets.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ETickets.ModelView
{
    public class ChairVM
    {
        [Required]
        public int? ChiarsNumber { get; set; }
        public int CinemaId { get; set; }
        public bool IsWorking { get; set; }
        public IEnumerable<SelectListItem>? Cinemas { get; set; }
    }
}
