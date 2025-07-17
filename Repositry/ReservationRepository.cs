using ETickets.Data;
using ETickets.Models;
using ETickets.Repositry.IRepositry;

namespace ETickets.Repositry
{
    public class ReservationRepository : Repository<Reservation> , IReservationRepository
    {
        public ReservationRepository(ApplicationDbContext db) :base(db) 
        {
            
        }
    }
}
