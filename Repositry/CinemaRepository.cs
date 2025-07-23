using ETickets.Data;
using ETickets.Repositry.IRepositry;
using ETickets.Models;

namespace ETickets.Repositry
{
    public class CinemaRepository : Repository<Cinema>  , ICinemaRepository
    {

        readonly public ApplicationDbContext _db;
        public CinemaRepository(ApplicationDbContext db) : base (db)
        {
            _db = db;
        }


    }
}
