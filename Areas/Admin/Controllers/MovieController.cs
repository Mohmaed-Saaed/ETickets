using ETickets.Helpers;
using ETickets.Models;
using ETickets.ModelView.Admin;
using ETickets.Repositry.IRepositry;
using ETickets.Servies.IServies;
using ETickets.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ETickets.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class MovieController : Controller
    {

        private readonly IMovieRepository _MovieRepository;
        private readonly IActorRepository _ActorRepository;
        private readonly IActorMovieRepository _ActorMovieRepository;

        private readonly IMovieAdminSaveService _MovieAdminSaveService;

        public MovieController(IMovieRepository movieRepository,
                               IActorRepository actorRepository,
                               IActorMovieRepository actorMovieRepository,
                               IMovieAdminSaveService movieAdminSaveService)
        {
            _MovieRepository = movieRepository;
            _ActorRepository = actorRepository;
            _ActorMovieRepository = actorMovieRepository;
            _MovieAdminSaveService = movieAdminSaveService;
        }
        [Authorize(Roles = $"{SD.SuperAdmin},{SD.Admin},{SD.Employee}")]
        public IActionResult Index()
        {
            var movies = _MovieRepository.Get();

            return View(movies);
        }
        [Authorize(Roles = $"{SD.SuperAdmin},{SD.Admin}")]
        public JsonResult GetActorData(int id)
        {
            if (id == 0)
            {
                return Json(new { status = false, message = "Bad request" });
            }

            var actorData = _ActorRepository.Get(x => x.Id == id).Select(x => new { image = Url.Content("~/images/cast/") + x.ProfilePicture, name = $"{x.FirstName} {x.LastName}", id }).FirstOrDefault();

            return Json(new { status = true, actorData });
        }
        [Authorize(Roles = $"{SD.SuperAdmin},{SD.Admin}")]
        public IActionResult Save(int id)
        {
            var adminMovieIndexVM = _MovieAdminSaveService.AdminMovieSaveVM(id);

            return View(adminMovieIndexVM);
        }
        [HttpPost]
        [Authorize(Roles = $"{SD.SuperAdmin},{SD.Admin}")]
        public IActionResult Save(AdminMovieSaveVM movieVm)
        {
            if (movieVm.ImgFile is not null)
            {
                FileSaveResult file = FileHelper.SaveFile(movieVm.ImgFile , "movies");

                movieVm.Movie.ImgUrl = file.FileName;
            }
            else
            {
                var MovieDb = _MovieRepository.GetOne(x => x.Id == movieVm.Movie.Id, null, true);

                if (MovieDb is not null)
                {

                    if (movieVm.IsMainImageRemoved)
                    {

                        if (MovieDb.ImgUrl is not null)
                             


                            FileHelper.RemoveFile(MovieDb.ImgUrl, "movies");

                    movieVm.Movie.ImgUrl = null;
                    } else
                    {
                        movieVm.Movie.ImgUrl = MovieDb.ImgUrl;
                    }

                }

            }
        
            if (movieVm.Movie.Id != 0)
            {
                _MovieRepository.Update(movieVm.Movie);
            }
            else
            {
                _MovieRepository.Create(movieVm.Movie);
            }

            if(movieVm.ImgFiles is not null && movieVm.ImgFiles.Count > 0)
            {

                List<MovieImage> movieImages = new List<MovieImage>();

                foreach (var movieImg in movieVm.ImgFiles)
                {

                 var file = FileHelper.SaveFile(movieImg , "movies");

                 movieImages.Add(new MovieImage { MovieId = movieVm.Movie.Id, ImageUrl = file.FileName });

                }
                _MovieRepository.SaveRangeMovieImage(movieImages);
                
            }

            if(movieVm.SubImagesRemoved is not null && movieVm.SubImagesRemoved.Count > 0)
            {
                var movieImages = _MovieRepository.GetMovieImages(m => movieVm.SubImagesRemoved.Contains(m.Id));

                if(movieImages is not null && movieImages.Count() > 0)
                {

                    foreach(var image in movieImages)
                    {
                        if (image.ImageUrl is not null)
                        {
                            FileHelper.RemoveFile(image.ImageUrl, "movies");
                        }
                    }

                    _MovieRepository.RemoveRangeMovieImage(movieImages);

                }
            }

            List<ActorMovie> actorMovies = new();

            if (movieVm.Movie.Id != 0)
            {

                var actorMovie = _ActorMovieRepository.Get(a => a.MovieId == movieVm.Movie.Id);

                if (actorMovie.Count() > 0)
                {
                    _ActorMovieRepository.RemoveRange(actorMovie);
                }

                if (movieVm.ActorIds is not null)
                {
                    foreach (var actorsId in movieVm.ActorIds)
                    {
                        _ActorMovieRepository.Create(new ActorMovie { ActorId = actorsId, MovieId = movieVm.Movie.Id });
                    }
                }

                if (actorMovies.Count > 0)
                {
                    _ActorMovieRepository.AddRange(actorMovies);
                }

            }
            else
            {
                if(movieVm.ActorIds is not null)
                {
                    foreach (var actorsId in movieVm.ActorIds)
                    {
                        _ActorMovieRepository.Create(new ActorMovie { ActorId = actorsId, MovieId = movieVm.Movie.Id });
                    }
                }
                _ActorMovieRepository.AddRange(actorMovies);
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = $"{SD.SuperAdmin},{SD.Admin}")]
        public IActionResult Delete(int id)
        {

            var movie = _MovieRepository.GetOne(x => x.Id == id);

            if (movie is not null)
            {
                if (movie.ImgUrl is not null)
                {
                    FileHelper.RemoveFile(movie.ImgUrl, "movies");
                }

                var movieImages = _MovieRepository.GetMovieImages(x => x.MovieId == id);


                if (movieImages is not null && movieImages.Count() > 0)
                {
                    foreach (var movieImg in movieImages)
                    {

                        if (movieImg.ImageUrl is not null)
                        {
                            FileHelper.RemoveFile(movieImg.ImageUrl, "movies");

                        }
                    }
                    _MovieRepository.RemoveRangeMovieImage(movieImages);

                }
                _MovieRepository.Delete(movie);
            }

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Assign(int Id)
        {
            if (Id > 0)
            {
                return RedirectToAction("Save", "MovieDay", new { area = "Admin"  , Id });
            }
            return NotFound();
        }


    }
}
