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
    [Authorize(Roles = $"{SD.SuperAdmin},{SD.Admin}")]
    public class ChairController : Controller
    {
        private readonly IChairRepository _ChairRepository;
        private readonly ICinemaRepository _CinemaRepository;
        public ChairController(IChairRepository chairRepository,
                                ICinemaRepository cinemaRepository)
        {
            _ChairRepository = chairRepository;
            _CinemaRepository = cinemaRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {

        var chairs =  _ChairRepository.Get(include: [x => x.Cinema]);

            return View(chairs);
        }

        [HttpGet]
        public IActionResult Save()
       {

            var cinema = _CinemaRepository.Get();

            var chiarVm = new ChairVM{
                Cinemas = cinema.Select(x => new SelectListItem { Value = x.Id.ToString() , Text = x.Name })
            };

            return View(chiarVm);
        }

        [HttpPost]
        public IActionResult Save(ChairVM chairVM)
        {
            var chairsToAdd = new List<Chair>();

            if(chairVM is not null)
            {
                for (int i = 0; i < chairVM.ChiarsNumber; i++)
                {
                    chairsToAdd.Add(new Chair
                    {
                        CinemaId = chairVM.CinemaId,
                        IsWorking = chairVM.IsWorking
                    });
                }

                _ChairRepository.AddRange(chairsToAdd);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult ChairStatus(int Id) 
        { 
            
           var chair = _ChairRepository.GetOne(x => x.Id == Id);

            if (chair is not null) {

                chair.IsWorking = !chair.IsWorking;

                _ChairRepository.Commit();
                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }
    }
}
