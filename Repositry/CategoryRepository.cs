using ETickets.Data;
using ETickets.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace ETickets.Repositry
{
    public class CategoryRepository
    {

        public readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool Create(Category category)
        {
            try
            {
                _db.Categories.Add(category);
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Update(Category category)
        {
            try
            {
                _db.Categories.Update(category);
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public bool Delete(Category category)
        {
            try
            {
                _db.Categories.Remove(category);
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public IEnumerable<Category> Get(Expression<Func<Category, bool>>? expression = null,
            Expression<Func<Category, object>>[]? include = null,
            bool tracked = false
            )
        {

            IQueryable<Category> categories = _db.Categories;

            if (expression is not null)
            {
                categories = categories.Where(expression);
                
            }

            if (include is not null)
            {
                foreach (var item in include)
                {
                    categories = categories.Include(item);
                }
            }

            if (tracked)
            {
                categories = categories.AsNoTracking();
            }



            return categories.ToList();
        }



        public Category? GetOne(Expression<Func<Category, bool>>? expression = null,
            Expression<Func<Category, object>>[]? include = null,
            bool tracked = false)
        {
            return Get(expression, include, tracked).FirstOrDefault();
        }

        public bool Commit()
        {
            try
            {
                _db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
