using ETickets.Models;
using System.Linq.Expressions;

namespace ETickets.Repositry.IRepositry
{
    public interface IMovieRepository : IRepository<Movie>
    {
        IEnumerable<MovieImage> GetMovieImages(Expression<Func<MovieImage, bool>>? expression = null);
        void SaveRangeMovieImage(IEnumerable<MovieImage> movieImages);
        void RemoveRangeMovieImage(IEnumerable<MovieImage> movieImages);
    }
}
