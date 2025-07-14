using ETickets.Data;
using ETickets.Models;
using ETickets.Repositry.IRepositry;

namespace ETickets.Repositry
{
    public class MovieDayRepository : Repository<MovieDay> , IMovieDayRepository
    {
        public MovieDayRepository(ApplicationDbContext db) : base (db) { 

        }   
    }
}
