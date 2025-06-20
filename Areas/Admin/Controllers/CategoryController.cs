using ETickets.Data;
using ETickets.Models;
using ETickets.ModelView.Admin;
using Microsoft.AspNetCore.Mvc;

namespace ETickets.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {

            var category = _db.Categories.ToList();
            return View(category);
        }
        [HttpGet]
        public IActionResult CreateEdit(int? id)
        {
            var Category = _db.Categories.FirstOrDefault(c => c.Id == id);


            return View(Category);
        }

        [HttpPost]
        public IActionResult CreateEdit(Category category)
        {

            if(category.Id != 0)
            {
                _db.Categories.Update(category);
            } else
            {
                _db.Categories.Add(category);
            }
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var Category = _db.Categories.FirstOrDefault(c => c.Id == id);

            if (Category is not null) {
                _db.Remove(Category);
                _db.SaveChanges();
             }

            return RedirectToAction(nameof(Index));
        }
    }
}
