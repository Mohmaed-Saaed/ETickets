using ETickets.Data;
using ETickets.Helpers;
using ETickets.Models;
using ETickets.Repositry.IRepositry;

namespace ETickets.Repositry
{
    public class ActosRepository  : Repository<Actor> , IActorRepository
    {

        public ActosRepository(ApplicationDbContext _db) : base(_db) {

        }

   

    }
}
