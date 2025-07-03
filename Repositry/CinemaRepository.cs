using ETickets.Data;
using ETickets.Repositry.IRepositry;
using ETickets.Models;

namespace ETickets.Repositry
{
    public class CinemaRepository : Repository<Cinema>  , ICinemaRepository
    {

        public CinemaRepository(ApplicationDbContext _db) : base (_db)
        {
            
        }
    }
}
