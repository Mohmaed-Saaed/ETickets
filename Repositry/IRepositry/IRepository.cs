using NPOI.SS.Formula.Functions;
using System.Linq.Expressions;

namespace ETickets.Repositry.IRepositry
{
    public interface IRepository<T> where T : class
    {
        bool Create(T entity);
        bool Update(T entity);

        bool Delete(T entity);

        IEnumerable<T> Get(Expression<Func<T, bool>>? expression = null,
           Expression<Func<T, object>>[]? include = null,
           bool tracked = false
           );

        T? GetOne(Expression<Func<T, bool>>? expression = null,
            Expression<Func<T, object>>[]? include = null,
            bool tracked = false);

        bool Commit();

    }
}
