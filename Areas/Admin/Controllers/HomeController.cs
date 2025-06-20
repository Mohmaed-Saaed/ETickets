using ETickets.Data;
using ETickets.ModelView.Admin;
using Microsoft.AspNetCore.Mvc;

namespace ETickets.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            AdminIndexVM indexVM = new AdminIndexVM
            {
                NumberOfMovies = _db.Movies.Count(),
                NumberOfActors = _db.Actors.Count(),
                NumberOfCategories = _db.Categories.Count(),
            };
            return View(indexVM);
        }
    }
}
