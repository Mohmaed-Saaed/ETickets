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

            var movieDayVM =   new MovieDaysVM()
            {
                MovieId = Id,
                MovieName = movie.Name,
                Date = DateOnly.FromDateTime(DateTime.Now)
            };

            return View(movieDayVM);        
        }

        public IActionResult GetCurrentMovieDayDatesForDay(DateOnly date, int id = 0 )
        {

            return new EmptyResult();
        }
    }
}
