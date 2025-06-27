using System.ComponentModel.DataAnnotations;

namespace ETickets.Models
{
    public class MovieImage
    {
        public int Id { get; set; }
        public int MovieId{ get; set; }

        [Display(Name = "Addtional images")]
        public string? ImageUrl{ get; set; }
        public Movie? Movie { get; set; }    
        
    }
}
