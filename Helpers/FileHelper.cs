using ETickets.Models;

namespace ETickets.Helpers
{
    public class FileHelper
    {
        private static string FileName(IFormFile formFile)
        {
            return Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);
        }

        public static string FilePath(string folderNameImages)
        {

            return Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\images\\{folderNameImages}\\");
        }

        public static FileSaveResult SaveFile(IFormFile formFile , string folderNameImages)
        {
            var fileName = FileName(formFile);

            var filePath = FilePath(folderNameImages);

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

        public static void RemoveFile(string fileName ,string folderNameImages)
        {
            var filePath = Path.Combine( FilePath(folderNameImages) ,fileName);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
        }
    }
}
