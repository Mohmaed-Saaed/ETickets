using ETickets.Data;
using ETickets.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ETickets.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ActorController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public ActorController(ApplicationDbContext db , IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {

            var Actors = _db.Actors.ToList();
            return View(Actors);
        }


        [HttpGet]
        public IActionResult Save(int id)
        {
            Actor actor = new Actor();

            if (_db.Actors.Any(a => a.Id == id))
            {
                actor = _db.Actors.FirstOrDefault(a => a.Id == id);
            }
            return View(actor);
        }

        [HttpPost]
        public IActionResult Save(Actor actor, IFormFile ProfilePicture)
        {
            var actorDb = _db.Actors.AsNoTracking().FirstOrDefault(a => a.Id == actor.Id);

            if (ProfilePicture is not null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfilePicture.FileName);

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cast\\");

                var filePathWithImage = Path.Combine(filePath, fileName);

                using (var stream = System.IO.File.Create(filePathWithImage))
                {
                    ProfilePicture.CopyTo(stream);
                }

                if (actorDb is not null)
                {
                    var oldfilePathWithImage = Path.Combine(filePath, actorDb.ProfilePicture);

                    if (System.IO.File.Exists(oldfilePathWithImage))
                    {
                        System.IO.File.Delete(oldfilePathWithImage);
                    }
                }

                actor.ProfilePicture = fileName;
            }

            if (actor.Id != 0)
            {
                _db.Actors.Update(actor);
            }
            else
            {
                _db.Actors.Add(actor);
            }
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));

        }
        public IActionResult Delete(int id)
        {
            if (_db.Actors.Any(a => a.Id == id))
            {
                var actor = _db.Actors.AsNoTracking().FirstOrDefault(a => a.Id == id);

                if(actor?.ProfilePicture is not null)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cast\\");

                    var oldfilePathWithImage = Path.Combine(filePath, actor.ProfilePicture);

                    if (System.IO.File.Exists(oldfilePathWithImage))
                    {
                        System.IO.File.Delete(oldfilePathWithImage);
                    }
                }

                _db.Remove(_db.Actors.FirstOrDefault(c => c.Id == id));

                _db.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
