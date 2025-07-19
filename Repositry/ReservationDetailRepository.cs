using ETickets.Data;
using ETickets.Models;
using ETickets.Repositry.IRepositry;

namespace ETickets.Repositry
{
    public class ReservationDetailRepository : Repository<ReservationDetail>, IReservationDetailRepository
    {

        public ReservationDetailRepository(ApplicationDbContext db) : base(db) { 
        
        }

    }
}
