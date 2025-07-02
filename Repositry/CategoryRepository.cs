using ETickets.Data;
using ETickets.Models;
using ETickets.Repositry.IRepositry;
using MathNet.Numerics.Statistics.Mcmc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.Linq;
using System.Linq.Expressions;

namespace ETickets.Repositry
{
    public class CategoryRepository : Repository<Category> , ICategoryRepository
    {

        public CategoryRepository(ApplicationDbContext _db) : base(_db) { 
        
        
        }
           
    }
}
