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

        public string FileName (IFormFile formFile)
        {
            return Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);
        }

        public string FilePath()
        {

            return Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cast\\");
        }

        public FileSaveResult SaveFile(IFormFile formFile) 
        {
            var fileName = FileName(formFile);

            var filePath = FilePath();

            var filePathWithName = Path.Combine(filePath, fileName);

            using (var stream = System.IO.File.Create(filePathWithName))
            {
                formFile.CopyTo(stream);
            }

            return new FileSaveResult
            {
                FileName = fileName,
                FilePath = filePath,
                FilePathWithName = filePathWithName
            };
        }

        public void RemoveOldFile(Actor actor , string filePath) {

            if (actor.ProfilePicture is not null)
            {
                var oldfilePathWithImage = Path.Combine(filePath, actor.ProfilePicture);

                if (System.IO.File.Exists(oldfilePathWithImage))
                {
                    System.IO.File.Delete(oldfilePathWithImage);
                }
            }

        }

    }
}
