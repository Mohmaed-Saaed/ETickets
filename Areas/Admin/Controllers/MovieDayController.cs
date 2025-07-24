using ETickets.Migrations;
using ETickets.Models;
using ETickets.ModelView;
using ETickets.Repositry.IRepositry;
using ETickets.Utilities;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ETickets.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =$"{SD.Admin},{SD.SuperAdmin}")]
    public class MovieDayController : Controller
    {
        readonly private IMovieDayRepository _MovieDayRepository;
        readonly private IMovieRepository _MovieRepository;
        readonly private IDayRepository _DayRepository;
        public MovieDayController(IMovieDayRepository movieDayRepository,
                                  IMovieRepository movieRepository,
                                  IDayRepository dayRepository)
        {
            _MovieDayRepository = movieDayRepository;
            _MovieRepository = movieRepository;
            _DayRepository = dayRepository;
        }
        public IActionResult Index()
        {
             var movieDays = _MovieDayRepository.Get();
            return View(movieDays);
        }

        [HttpGet]
        public IActionResult Assign(int Id) {
            var movie = _MovieRepository.GetOne(x => x.Id == Id);
            
            if (movie == null)
            {
                return RedirectToAction("Index", "Movie", new { area = "Admin"});
            }

            var movieDayVM = new MovieDaysVM()
            {
                MovieId = Id,
                MovieStartDate = movie.StartDate,
                MovieEndDate = movie.EndDate,
                MovieName = movie.Name,
                Date = movie.StartDate,
                Days = _DayRepository.Get().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList(),
                Movie = movie
            };

            return View(movieDayVM);        
        }

        [HttpPost]
        public IActionResult Assign(MovieDaysVM movieDaysVM)
        {
            var movieDays = _MovieDayRepository.Get(x => x.Date == movieDaysVM.Date);


            var isDateTaken = movieDays.Any(x => movieDaysVM.Date.ToDateTime(movieDaysVM.From) 
                                         < x.Date.ToDateTime(x.To.AddMinutes(x.MinutesRestAfterMovie)) && 
                                         x.Date.ToDateTime(x.From) < 
                                         movieDaysVM.Date.ToDateTime(movieDaysVM.To.AddMinutes(movieDaysVM.MinutesRestAfterMovie)));

            if(isDateTaken)
            {
                TempData["success"] = "Date is reserved";
                var movieDay = movieDaysVM.Adapt<MovieDay>();
                _MovieDayRepository.Create(movieDay);
                return View();
            }

            TempData["error"] = "date is already taken by another movie";

            return new EmptyResult();
        }

        [HttpGet]
        public IActionResult GetMovieDateTimes(DateOnly date, int movieId = 0 )
        {
            var movieDay = _MovieDayRepository.Get(x => x.Date == date, include: [x => x.Movie]);
            if (movieDay is not null)
            {

                return PartialView("ShowMovieTimes", movieDay);
            }

            return new EmptyResult();
        }
    }
}
