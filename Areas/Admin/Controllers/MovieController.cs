using ETickets.Data;
using ETickets.Migrations;
using ETickets.Models;
using ETickets.ModelView.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ETickets.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class MovieController : Controller
    {

        private readonly ApplicationDbContext _db;

        public MovieController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var movies = _db.Movies.ToList();

            return View(movies);
        }

        public JsonResult GetActorData(int id)
        {
            if (id == 0)
            {
                return Json(new { status = false, message = "Bad request" });
            }

            var actorData = _db.Actors.Where(x => x.Id == id)
                .Select(x => new { image = Url.Content("~/images/cast/") + x.ProfilePicture, name = $"{x.FirstName} {x.LastName}", id }).FirstOrDefault();

            return Json(new { status = true, actorData });
        }

        public IActionResult Save(int id)
        {
            Movie movie = new Movie { EndDate = DateTime.Now, StartDate = DateTime.Now };

            if (_db.Movies.Any(m => m.Id == id))
            {
                movie = _db.Movies.FirstOrDefault(m => m.Id == id);
            }
            ;
            var selectedActorIds = _db.ActorMovies
             .Where(am => am.MovieId == id)
             .Select(am => am.ActorId)
             .ToList();

            AdminMovieSaveVM adminMovieIndexVM = new AdminMovieSaveVM()
            {
                MovieStatus = _db.MovieDisplayStates.Select(x => new SelectListItem { Text = x.Status, Value = x.Id.ToString() }).ToList(),

                Categories = _db.Categories.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList(),

                Cinemas = _db.Cinemas.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList(),

                Actors = _db.Actors.Select(x => new SelectListItem { Text = $"{x.FirstName} {x.LastName}", Value = x.Id.ToString() }).ToList(),

                ActorsSelectList = _db.ActorMovies.Where(x => x.MovieId == id)
                .Select(x => new SelectListItem
                {
                    Text = $"{x.Actor.FirstName} {x.Actor.LastName}",
                    Value = x.Actor.Id
                   .ToString(),
                    Selected = selectedActorIds.Contains(x.ActorId)
                }).ToList(),

                Movie = movie,

                ActorIds = selectedActorIds,
                SubImages = _db.MovieImages.Where(x => x.MovieId == id).ToList()
            };
            return View(adminMovieIndexVM);
        }
        [HttpPost]
        public IActionResult Save(AdminMovieSaveVM movieVm)
        {
            if (movieVm.ImgFile is not null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(movieVm.ImgFile.FileName);

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies\\");

                var filePathWithFileName = Path.Combine(filePath, fileName);

                using (var createFile = System.IO.File.Create(filePathWithFileName))
                {
                    movieVm.ImgFile.CopyTo(createFile);
                }

                movieVm.Movie.ImgUrl = fileName;
            }
            else
            {
                var MovieDb = _db.Movies.AsNoTracking().FirstOrDefault(m => m.Id == movieVm.Movie.Id);

                if (MovieDb is not null)
                {
                    if (movieVm.IsMainImageRemoved)
                {

                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies\\");

                    if (MovieDb.ImgUrl is not null)
                    {
                        var filePathWithFileName = Path.Combine(filePath, MovieDb.ImgUrl);

                        if (System.IO.File.Exists(filePathWithFileName))
                        {
                            System.IO.File.Delete(filePathWithFileName);
                        }
                    }
                    movieVm.Movie.ImgUrl = null;


          
                }
                else
                    {
                        movieVm.Movie.ImgUrl = MovieDb.ImgUrl;
                     }
                }
            }

            if (movieVm.Movie.Id != 0)
            {
                _db.Movies.Update(movieVm.Movie);

                _db.SaveChanges();
            }
            else
            {
                _db.Movies.Add(movieVm.Movie);

                _db.SaveChanges();
            }




            if(movieVm.ImgFiles is not null && movieVm.ImgFiles.Count > 0)
            {

                List<MovieImage> movieImages = new List<MovieImage>();

                foreach (var movieImg in movieVm.ImgFiles)
                {

                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(movieImg.FileName);

                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies\\");

                        var filePathWithFileName = Path.Combine(filePath, fileName);

                        using (var createFile = System.IO.File.Create(filePathWithFileName))
                        {
                            movieImg.CopyTo(createFile);
                        }

                    movieImages.Add(new MovieImage { MovieId = movieVm.Movie.Id, ImageUrl = fileName });

                }

                _db.MovieImages.AddRange(movieImages);

                _db.SaveChanges();

            }


            if(movieVm.SubImagesRemoved is not null && movieVm.SubImagesRemoved.Count > 0)
            {
                var movieImages = _db.MovieImages.Where(m => movieVm.SubImagesRemoved.Contains(m.Id)).ToList();


                if(movieImages is not null && movieImages.Count > 0)
                {


                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies\\");



                    foreach(var image in movieImages)
                    {
                        if (image.ImageUrl is not null)
                        {
                            var filePathWithFileName = Path.Combine(filePath, image.ImageUrl);

                            if (System.IO.File.Exists(filePathWithFileName))
                            {
                                System.IO.File.Delete(filePathWithFileName);
                            }
                        }
                    }
     
                    _db.MovieImages.RemoveRange(movieImages);

                    _db.SaveChanges();

                }


            }

            List<ActorMovie> actorMovies = new();

            if (movieVm.Movie.Id != 0)
            {

                var actorMovie = _db.ActorMovies.Where(a => a.MovieId == movieVm.Movie.Id).ToList();

                if (actorMovie.Count > 0)
                {
                    _db.ActorMovies.RemoveRange(actorMovie);

                    _db.SaveChanges();
                }

                if (movieVm.ActorIds is not null)
                {
                    foreach (var actorsId in movieVm.ActorIds)
                    {
                        actorMovies.Add(
                            new ActorMovie { ActorId = actorsId, MovieId = movieVm.Movie.Id }
                            );
                    }
                }

                if (actorMovies.Count > 0)
                {
                    _db.ActorMovies.AddRange(actorMovies);
                }

            }
            else
            {
                foreach (var actorsId in movieVm.ActorIds)
                {
                    actorMovies.Add(
                        new ActorMovie { ActorId = actorsId, MovieId = movieVm.Movie.Id }
                        );
                }

                _db.ActorMovies.AddRange(actorMovies);
            }

            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            if (_db.Movies.Any(m => m.Id == id))
            {
                var movie = _db.Movies.First(m => m.Id == id);

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies\\");
                if (movie.ImgUrl is not null)
                {

                    var filePathWithFileName = Path.Combine(filePath, movie.ImgUrl);

                    if (System.IO.File.Exists(filePathWithFileName))
                    {
                        System.IO.File.Delete(filePathWithFileName);
                    }
                }

                var movieImages = _db.MovieImages.Where(m => m.MovieId == id).ToList();


                if(movieImages is not null && movieImages.Count > 0)
                {

                    foreach(var movieImg in movieImages)
                    {

                        if(movieImg.ImageUrl is not null)
                        {
                            var filePathWithFileName = Path.Combine(filePath, movieImg.ImageUrl);

                            if (System.IO.File.Exists(filePathWithFileName))
                            {
                                System.IO.File.Delete(filePathWithFileName);
                            }
                        }
                    }
                    _db.MovieImages.RemoveRange(movieImages);

                    _db.SaveChanges();
                }
                _db.Movies.Remove(movie);

                _db.SaveChanges();

            }

            return RedirectToAction(nameof(Index));
        }

    }
}
