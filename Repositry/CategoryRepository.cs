using ETickets.Data;
using ETickets.Models;
using ETickets.Repositry.IRepositry;


namespace ETickets.Repositry
{
    public class CategoryRepository : Repository<Category> , ICategoryRepository
    {

        public CategoryRepository(ApplicationDbContext _db) : base(_db) { 
        
        
        }
           
    }
}
