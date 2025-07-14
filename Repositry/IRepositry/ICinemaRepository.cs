using ETickets.Models;

namespace ETickets.Repositry.IRepositry
{
    public interface ICinemaRepository : IRepository<Cinema>
    {

        void AddChairs(int cinemaId, int numberOfChairs);
        void DeleteChairs(int cinemaId);
    }
}
