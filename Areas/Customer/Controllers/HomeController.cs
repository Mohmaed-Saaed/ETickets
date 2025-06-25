using System.Diagnostics;
using System.Threading.Tasks;
using ETickets.Data;
using ETickets.Models;
using ETickets.ModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;


namespace ETickets.Areas.Customer.Controllers
{

    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        // Updated constructor to initialize both fields
        public HomeController( ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index(IndexVM indexVM)
        {
            IQueryable<Movie> MoviesList = _db.Movies.Include(m => m.Category).Include(m => m.Cinema).Where(m => m.MovieStatus == true && m.Name.Contains(indexVM.Search ?? ""));

            if (indexVM.CategoryId != 0)
            {
                MoviesList = MoviesList.Where(m => m.CategoryId == indexVM.CategoryId);
            }

            if (indexVM.CinemaId != 0)
            {
                MoviesList = MoviesList.Where(m => m.CinemaId == indexVM.CinemaId);
            }
            
                var IndexMovie = new IndexVM
                {
                    Movies = MoviesList.ToList(),
                    Categories = _db.Categories.Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }).ToList(),
                    Cinemas = _db.Cinemas.Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }).ToList(),
                    Search = indexVM.Search
                };

            return View(IndexMovie);
        }

        public IActionResult Categories()
        {
            var Categories = _db.Categories.ToList();
            return View(Categories);
        }
        public IActionResult Cinemas()
        {
            var Cinemas = _db.Cinemas.ToList();
            return View(Cinemas);
        }

        public async Task<string> GetYoutubeTrailer(string movieName)
            {

            try
            {
                var apiKey = "AIzaSyBTgj3GZ2iFStKkwG8vI6t0Lnc2dEeozKY";
                var searchQuery = Uri.EscapeDataString(movieName + " trailer");
                var url = $"https://www.googleapis.com/youtube/v3/search?part=snippet&q={searchQuery}&key={apiKey}&maxResults=1&type=video";

                using var client = new HttpClient();
                var response = await client.GetStringAsync(url);
                var json = JObject.Parse(response);

                var videoId = json["items"]?[0]?["id"]?["videoId"]?.ToString();

                if (!string.IsNullOrEmpty(videoId))
                {
                    return $"https://www.youtube.com/embed/{videoId}";
                }

            } catch(Exception ex)
            {

            }
             
                return "https://www.youtube.com"; 
         }

    public async Task<IActionResult> Details(int Id)
        {
            var movie = _db.Movies
                           .Include(m => m.ActorMovies)
                           .ThenInclude(m => m.Actor)
                           .Include(m => m.Category)       
                           .Include(m => m.Cinema) 
                           .Include(m => m.Category) 
                           .FirstOrDefault(m => m.Id == Id);

            ViewBag.srcYoutube  = await GetYoutubeTrailer(movie.Name);
            return View(movie);
        }


        public IActionResult ActorProfile(int Id)
        {
         var Actor =    _db.Actors.FirstOrDefault(a => a.Id == Id);
            return View(Actor);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
