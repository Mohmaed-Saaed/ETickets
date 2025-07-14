using ETickets.Models;
using ETickets.ModelView;
using ETickets.Repositry.IRepositry;
using Microsoft.AspNetCore.Mvc;

namespace ETickets.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MovieDayController : Controller
    {
        readonly private IMovieDayRepository _MovieDayRepository;
        readonly private IMovieRepository _MovieRepository;
        public MovieDayController(IMovieDayRepository movieDayRepository,
                                  IMovieRepository movieRepository)
        {
            _MovieDayRepository = movieDayRepository;
            _MovieRepository = movieRepository; 
        }
        public IActionResult Index()
        {
             var movieDays = _MovieDayRepository.Get();
            return View(movieDays);
        }


        public IActionResult Assign(int Id) {
            var movie = _MovieRepository.GetOne(x => x.Id == Id);
            
            if (movie == null)
            {
                return RedirectToAction("Index", "Movie", new { area = "Admin"});
            }

            //var CountDays = null;

            //DateTime.
            //List<DateTime> Days = new List<DateTime>();

            //foreach(var day in movie.) { }



            var movieDayVM =   new MovieDaysVM()
            {
                MovieName = movie.Name
            };

            return View(movieDayVM);        
        }
    }
}
