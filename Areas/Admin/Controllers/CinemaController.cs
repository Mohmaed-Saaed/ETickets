using ETickets.Models;
using ETickets.Repositry;
using ETickets.Repositry.IRepositry;
using ETickets.Utilities;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = $"{SD.SuperAdmin},{SD.Admin},{SD.Employee}")]
        public IActionResult Index()
        {
            var Cinema = _CinemaRepository.Get();

            return View(Cinema);
        }

        [HttpGet]
        [Authorize(Roles = $"{SD.SuperAdmin},{SD.Admin}")]
        public IActionResult Save(int? id)
        {
            Cinema Cinema = _CinemaRepository.GetOne(x => x.Id == id) ?? new Cinema();
            return View(Cinema);
        }

        [HttpPost]
        [Authorize(Roles = $"{SD.SuperAdmin},{SD.Admin}")]
        public IActionResult Save(Cinema cinema)
        {

            if (cinema.Id != 0)
            {
                _CinemaRepository.Update(cinema);
                _CinemaRepository.DeleteChairs(cinema.Id);
                _CinemaRepository.AddChairs(cinema.Id, cinema.NumberOfChairs);
            }
            else
            {
                _CinemaRepository.Create(cinema);

                var cinemaId = _CinemaRepository.GetOne(x => x.Name == cinema.Name)!.Id ;
                _CinemaRepository.AddChairs(cinemaId , cinema.NumberOfChairs);

            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = $"{SD.SuperAdmin},{SD.Admin}")]
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
