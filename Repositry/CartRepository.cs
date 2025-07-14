using ETickets.Data;
using ETickets.Models;
using ETickets.Repositry.IRepositry;

namespace ETickets.Repositry
{
    public class CartRepository  : Repository<Cart> ,  ICartRepository
    {
        public CartRepository(ApplicationDbContext db) : base(db)
        {

        }


    }
}
