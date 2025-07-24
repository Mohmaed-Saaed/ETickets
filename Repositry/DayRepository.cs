using ETickets.Data;
using ETickets.Models;
using ETickets.Repositry.IRepositry;

namespace ETickets.Repositry
{
    public class DayRepository : Repository<Day> , IDayRepository
    {

        public DayRepository(ApplicationDbContext db) :base(db) { 
        }

    }
}
