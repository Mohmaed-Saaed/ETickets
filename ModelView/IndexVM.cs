using ETickets.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ETickets.ModelView
{
    public class IndexVM
    {
        public string? Search{ get; set; }

        public int CategoryId { get; set; }
        public List<SelectListItem>? Categories{ get; set; }

        public int CinemaId { get; set; }
        public List<SelectListItem>? Cinemas{ get; set; }
        public ICollection<Movie> Movies{ get; set; } = new List<Movie>();
    }
}
