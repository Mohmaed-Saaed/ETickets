using ETickets.Data;
using ETickets.Models;
using ETickets.Repositry.IRepositry;
using NPOI.SS.Formula.Functions;
using System.Linq;
using System.Linq.Expressions;

namespace ETickets.Repositry
{
    public class MovieRepository : Repository<Movie> , IMovieRepository
    {

        public readonly ApplicationDbContext _db;
        public MovieRepository(ApplicationDbContext db) : base(db) {

            _db = db;
        }

        public IEnumerable<MovieImage> GetMovieImages(Expression<Func<MovieImage, bool>>? expression = null)
        {
            IQueryable<MovieImage> movieImage = _db.MovieImages;

            if (expression is not null) {

                movieImage = movieImage.Where(expression);
            }

            return movieImage.ToList();
        }


        public void SaveRangeMovieImage(IEnumerable<MovieImage> movieImages)
        {
            try
            {
                _db.MovieImages.AddRange(movieImages);
                Commit();
            }
            catch (Exception ex) { 
            
            }
        }

        public void RemoveRangeMovieImage(IEnumerable<MovieImage> movieImages)
        {
            try
            {
                _db.MovieImages.RemoveRange(movieImages);
                Commit();
            }
            catch (Exception ex) { 

            }
        }
    }
}
