using ETickets.Data;
using ETickets.Models;
using ETickets.Repositry.IRepositry;

namespace ETickets.Repositry
{
    public class MovieDisplayStateRepository : Repository<MovieDisplayState> , IMovieDisplayStateRepository
    {
        public MovieDisplayStateRepository(ApplicationDbContext _db) : base(_db)
        {

        }
    }
}
