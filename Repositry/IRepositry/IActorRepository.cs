using ETickets.Helpers;
using ETickets.Models;

namespace ETickets.Repositry.IRepositry
{
    public interface IActorRepository : IRepository<Actor>
    {
        //FileSaveResult SaveFile(IFormFile formFile);
        //string FilePath();
        //void RemoveOldFile(Actor actor, string filePath);
    }
}

