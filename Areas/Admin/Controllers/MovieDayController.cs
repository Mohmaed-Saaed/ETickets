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
             var movieDays = _MovieDayRepository.Get(include: [x =>x.Movie , x  => x.Day]);
            return View(movieDays);
        }

        [HttpGet]
        public IActionResult Assign(int Id , int actionType = 1) {

            if(actionType == 1 && Id != 0)
            {
                var movie = _MovieRepository.GetOne(x => x.Id == Id);

                if (movie == null)
                {
                    return RedirectToAction("Index", "Movie", new { area = "Admin" });
                }
                var movieDayVM = new MovieDaysVM()
                {
                    MovieId = Id,
                    MovieStartDate = movie.StartDate,
                    MovieEndDate = movie.EndDate,
                    MovieName = movie.Name,
                    Date = movie.StartDate,
                    Duration = movie.Duration,
                    Days = _DayRepository.Get().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList(),
                };

                return View(movieDayVM);

            } else if(actionType == 2 && Id != 0)
            {
                var movieDay = _MovieDayRepository.GetOne(x => x.Id == Id);

                if(movieDay is not null)
                {

                var movie = _MovieRepository.GetOne(x => x.Id == movieDay.MovieId);

                    if (movie is null)
                        return View(movieDay);

                    var movieDayVM = new MovieDaysVM()
                    {
                        MovieDayId = movieDay.Id,
                        MovieId = movie.Id,
                        MovieStartDate = movie.StartDate,
                        MovieEndDate = movie.EndDate,
                        MovieName = movie.Name,
                        Date = movieDay.Date,
                        Duration = movie.Duration,
                        From = movieDay.From,
                        To = movieDay.To,
                        MinutesRestAfterMovie = movieDay.MinutesRestAfterMovie,
                        DayId = movieDay.DayId,
                        Days = _DayRepository.Get().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList()
                    };

                    return View(movieDayVM);
                }                
            }

            return new EmptyResult();
        }

        [HttpPost]
        public IActionResult Assign(MovieDaysVM movieDaysVM)
        {

            movieDaysVM.Days = _DayRepository.Get().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();

            if(movieDaysVM.Date < movieDaysVM.MovieStartDate || movieDaysVM.Date > movieDaysVM.MovieEndDate)
            {
                TempData["error"] = "Wrong date";
                return View(movieDaysVM);
            }

            var fromTime = movieDaysVM.From.ToTimeSpan();
            var toTime = movieDaysVM.To.ToTimeSpan();

            var calcTime =  (toTime - fromTime) != movieDaysVM.Duration;

            if (calcTime) {
                  TempData["error"] = "Wrong movie Duration time";
                  return View(movieDaysVM);
            }

            var movieDays = _MovieDayRepository.Get(x => x.Date == movieDaysVM.Date , tracked:true);

            var isDateTaken = movieDays.Any(x => movieDaysVM.Date.ToDateTime(movieDaysVM.From) 
                                         < x.Date.ToDateTime(x.To.AddMinutes(x.MinutesRestAfterMovie)) && 
                                         x.Date.ToDateTime(x.From) < 
                                         movieDaysVM.Date.ToDateTime(movieDaysVM.To.AddMinutes(movieDaysVM.MinutesRestAfterMovie)));


            if(!isDateTaken)
            {
                if(movieDaysVM.MovieDayId != 0)
                {
                    var movieDay = _MovieDayRepository.GetOne(x => x.Id == movieDaysVM.MovieDayId);
                    if(movieDay == null) 
                        return View(movieDaysVM);
                    movieDay.From = movieDaysVM.From;
                    movieDay.To= movieDaysVM.To;
                    movieDay.Date= movieDaysVM.Date;
                    movieDay.DayId= (int)movieDaysVM.DayId;

                    TempData["success"] = "Date is updated";
                    _MovieDayRepository.Update(movieDay);

                } else
                {
                 var movieDay = movieDaysVM.Adapt<MovieDay>();

                 TempData["success"] = "Date is reserved";
                _MovieDayRepository.Create(movieDay);
                }
                return View(movieDaysVM);
            }

            TempData["error"] = "date is already taken by another movie";

            return View(movieDaysVM);
        }

        [HttpGet]
        public IActionResult GetMovieDateTimes(DateOnly date, int movieId = 0 )
        {
            var movieDay = _MovieDayRepository.Get(x => x.Date == date, include: [x => x.Movie]);

         var order =   movieDay.OrderByDescending(x => x.Date);
            if (movieDay is not null)
            {

                return PartialView("ShowMovieTimes", movieDay);
            }

            return new EmptyResult();
        }
    }
}
