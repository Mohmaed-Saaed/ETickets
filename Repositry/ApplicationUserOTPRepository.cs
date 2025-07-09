using ETickets.Data;
using ETickets.Models;
using ETickets.Repositry.IRepositry;

namespace ETickets.Repositry
{
    public class ApplicationUserOTPRepository : Repository<ApplicationUserOTP> , IApplicationUserOTPRepository
    {
        public ApplicationUserOTPRepository(ApplicationDbContext db) :base(db) { 
        }

    }
}
