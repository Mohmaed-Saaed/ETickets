using ETickets.Models;
using ETickets.Repositry;
using ETickets.Repositry.IRepositry;
using Microsoft.AspNetCore.Mvc;

namespace ETickets.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CinemaController : Controller
    {

        private readonly ICinemaRepository _CinemaRepository;

        public CinemaController(ICinemaRepository CinemaRepository)
        {
            _CinemaRepository = CinemaRepository;
        }


        public IActionResult Index()
        {
            var Cinema = _CinemaRepository.Get();

            return View(Cinema);
        }

        [HttpGet]
        public IActionResult Save(int? id)
        {


            Cinema Cinema = _CinemaRepository.GetOne(x => x.Id == id) ?? new Cinema();



            return View(Cinema);
        }

        [HttpPost]
        public IActionResult Save(Cinema cinema)
        {

            if (cinema.Id != 0)
            {
                _CinemaRepository.Update(cinema);
            }
            else
            {
                _CinemaRepository.Create(cinema);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var cinema = _CinemaRepository.GetOne(c => c.Id == id);

            if (cinema is not null)
            {
                _CinemaRepository.Delete(cinema);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
