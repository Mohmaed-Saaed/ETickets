using ETickets.Helpers;
using ETickets.Models;
using ETickets.Repositry.IRepositry;
using ETickets.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [Authorize(Roles = $"{SD.SuperAdmin},{SD.Admin},{SD.Employee}")]
        public IActionResult Index()
        {

            var Actors = _ActorRepository.Get();
            return View(Actors);
        }

        [HttpGet]
        [Authorize(Roles = $"{SD.SuperAdmin},{SD.Admin}")]
        public IActionResult Save(int id)
        {
            Actor actor = _ActorRepository.GetOne(x => x.Id == id) ?? new Actor();

            if (actor is not null)
            {
            return View(actor);
            }

            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = $"{SD.SuperAdmin},{SD.Admin}")]
        public IActionResult Save(Actor actor, IFormFile ProfilePicture)
        {
            var actorDb = _ActorRepository.GetOne(x => x.Id == actor.Id , null , true);

            if (ProfilePicture is not null)
            {
                FileSaveResult file = FileHelper.SaveFile(ProfilePicture , "cast");

                if (actorDb is not null && file.FileName is not null)
                {
                    FileHelper.RemoveFile(file.FileName, "cast");
                }

                actor.ProfilePicture = file.FileName;
            }

            if (actor.Id != 0)
            {
                TempData["success"] = "Actor added successfully";
                _ActorRepository.Update(actor);
            }
            else
            {
                TempData["success"] = "Actor added successfully";
                _ActorRepository.Create(actor);
            }

            return RedirectToAction(nameof(Index));

        }
        [Authorize(Roles = $"{SD.SuperAdmin},{SD.Admin}")]
        public IActionResult Delete(int id)
        {
       
            var actor = _ActorRepository.GetOne(x => x.Id == id);

            if (actor is not null) {

                _ActorRepository.Delete(actor);

                if (actor?.ProfilePicture is not null)
                {
                    FileHelper.RemoveFile(actor.ProfilePicture, "cast");
                }

            }

            return RedirectToAction(nameof(Index));
        }
    }
}
