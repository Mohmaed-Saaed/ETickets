using ETickets.Data;
using ETickets.Models;
using ETickets.ModelView.Admin;
using ETickets.Repositry;
using ETickets.Repositry.IRepositry;
using Microsoft.AspNetCore.Mvc;

namespace ETickets.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }


        public IActionResult Index()
        {

            var category = _categoryRepository.Get();

            return View(category);
        }
        [HttpGet]
        public IActionResult Save(int? id)
        {
        
            
            Category  category = _categoryRepository.GetOne(x => x.Id == id) ?? new Category();
            


            return View(category);
        }

        [HttpPost]
        public IActionResult Save(Category category)
        {

            if(category.Id != 0)
            {
               _categoryRepository.Update(category);
            } else
            {
                _categoryRepository.Create(category);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var Category =  _categoryRepository.GetOne(c => c.Id == id);

            if (Category is not null) {
                _categoryRepository.Delete(Category);
             }

            return RedirectToAction(nameof(Index));
        }
    }
}
