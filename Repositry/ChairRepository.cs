using ETickets.Data;
using ETickets.Models;
using ETickets.Repositry.IRepositry;

namespace ETickets.Repositry
{
    public class ChairRepository : Repository<Chair>, IChairRepository
    {
        public ChairRepository(ApplicationDbContext db) :base(db) { 
        
        }
    }
}
