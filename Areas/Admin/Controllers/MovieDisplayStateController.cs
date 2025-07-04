using ETickets.Models;
using ETickets.Repositry.IRepositry;
using Microsoft.AspNetCore.Mvc;

namespace ETickets.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MovieDisplayStateController : Controller
    {
        private readonly IMovieDisplayStateRepository _MovieDisplayStateRepository;

        public MovieDisplayStateController(IMovieDisplayStateRepository MovieDisplayState)
        {
            _MovieDisplayStateRepository = MovieDisplayState;
        }

        public IActionResult Index()
        {
            var MovieDisplayState = _MovieDisplayStateRepository.Get();

            return View(MovieDisplayState);
        }

        [HttpGet]
        public IActionResult Save(int? id)
        {


            MovieDisplayState MovieDisplayState = _MovieDisplayStateRepository.GetOne(x => x.Id == id) ?? new MovieDisplayState();



            return View(MovieDisplayState);
        }

        [HttpPost]
        public IActionResult Save(MovieDisplayState MovieDisplayState)
        {

            if (MovieDisplayState.Id != 0)
            {
                _MovieDisplayStateRepository.Update(MovieDisplayState);
            }
            else
            {
                _MovieDisplayStateRepository.Create(MovieDisplayState);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var MovieDisplayState = _MovieDisplayStateRepository.GetOne(c => c.Id == id);

            if (MovieDisplayState is not null)
            {
                _MovieDisplayStateRepository.Delete(MovieDisplayState);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
