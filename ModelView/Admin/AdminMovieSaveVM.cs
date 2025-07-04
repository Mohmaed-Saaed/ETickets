using ETickets.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ETickets.ModelView.Admin
{
    public class AdminMovieSaveVM
    {
        public IEnumerable<SelectListItem>? MovieStatus { get; set; }
        public IEnumerable<SelectListItem>? Categories { get; set; }
        public IEnumerable<SelectListItem>? Cinemas { get; set; }
        public IEnumerable<SelectListItem>? Actors { get; set; }
        public IEnumerable<SelectListItem>? ActorsSelectList { get; set; }
        public List<int>? SubImagesRemoved { get; set; }
        public List<int?>? ActorIds{ get; set; }
        public IFormFile? ImgFile { get; set; }
        public List<IFormFile>? ImgFiles { get; set; }
        public Movie Movie { get; set; } = new Movie();  
        public Actor Actor { get; set; } = new Actor();  
        public MovieImage? MovieImage { get; set; }   
        public IEnumerable<MovieImage>  SubImages { get; set; } = new List<MovieImage>();  

        public bool IsMainImageRemoved { get; set; }

    }
}
