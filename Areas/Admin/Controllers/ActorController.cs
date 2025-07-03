using ETickets.Data;
using ETickets.Helpers;
using ETickets.Models;
using ETickets.Repositry;
using ETickets.Repositry.IRepositry;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ETickets.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ActorController : Controller
    {
        private readonly IActorRepository _ActorRepository;


        public ActorController(IActorRepository ActorRepository)
        {
            _ActorRepository = ActorRepository;
        }
        public IActionResult Index()
        {

            var Actors = _ActorRepository.Get();
            return View(Actors);
        }


        [HttpGet]
        public IActionResult Save(int id)
        {
            Actor actor = _ActorRepository.GetOne(x => x.Id == id);

            if (actor is not null)
            {
            return View(actor);
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult Save(Actor actor, IFormFile ProfilePicture)
        {
            var actorDb = _ActorRepository.GetOne(x => x.Id == actor.Id , null , true);

            if (ProfilePicture is not null)
            {
                FileSaveResult file = _ActorRepository.SaveFile(ProfilePicture);

                if (actorDb is not null && file.FilePath is not null)
                {
                    _ActorRepository.RemoveOldFile(actorDb , file.FilePath);
                }

                actor.ProfilePicture = file.FileName;
            }

            if (actor.Id != 0)
            {
                _ActorRepository.Update(actor);
            }
            else
            {
                _ActorRepository.Create(actor);
            }

            return RedirectToAction(nameof(Index));

        }
        public IActionResult Delete(int id)
        {
       
            var actor = _ActorRepository.GetOne(x => x.Id == id);

            if (actor is not null) {


                if (actor?.ProfilePicture is not null)
                {
                    _ActorRepository.RemoveOldFile(actor, _ActorRepository.FilePath());
                }

                _ActorRepository.Delete(actor);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
