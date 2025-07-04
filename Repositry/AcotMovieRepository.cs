using ETickets.Data;
using ETickets.Helpers;
using ETickets.Models;
using ETickets.Repositry.IRepositry;

namespace ETickets.Repositry
{
    public class AcotMovieRepository : Repository<ActorMovie>, IActorMovieRepository
    {

        public readonly ApplicationDbContext _db;

        public AcotMovieRepository(ApplicationDbContext db) : base (db) {
            _db = db;
        }

    }
}
