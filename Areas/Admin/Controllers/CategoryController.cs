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
        public IActionResult Save(int? id)
        {
            Category category = new Category();

            if (_db.Categories.Any(a => a.Id == id))
            {
                category = _db.Categories.FirstOrDefault(a => a.Id == id);
            }

            return View(category);
        }

        [HttpPost]
        public IActionResult Save(Category category)
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
