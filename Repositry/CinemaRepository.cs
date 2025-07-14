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

        public void AddChairs(int cinemaId, int numberOfChairs)
        {

            if(numberOfChairs > 0)
            {
                for (int i = 1; i <= numberOfChairs; i++)
                {
                    _db.Chairs.Add(new Chair { CinemaId = cinemaId });
                }

                Commit();
            }
    
        }

        public void DeleteChairs(int cinemaId)
        {
            if(cinemaId > 0)
            {
                var cinemas = _db.Chairs.Where(x => x.CinemaId == cinemaId).ToList();

                if(cinemas.Count > 0)
                {
                    _db.Chairs.RemoveRange(cinemas);
                }
            }
        }

    }
}
