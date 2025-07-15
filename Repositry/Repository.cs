using ETickets.Data;
using ETickets.Repositry.IRepositry;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Operators.Utilities;
using System.Linq.Expressions;

namespace ETickets.Repositry
{
    public class Repository<T> : IRepository<T> where T : class
    {

        private readonly ApplicationDbContext _db;
        private readonly DbSet<T> _entity;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            _entity = _db.Set<T>();
        }
        public bool Create(T entity)
        {
            try
            {
                _entity.Add(entity);
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Update(T entity)
        {
            try
            {
                _entity.Update(entity);
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool Delete(T entity)
        {
            try
            {
                _entity.Remove(entity);
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public IEnumerable<T> Get(Expression<Func<T, bool>>? expression = null,
            Expression<Func<T, object>>[]? include = null,
             bool tracked = false
            )
        {

            IQueryable<T> entities = _entity;

            if (expression is not null)
            {
                entities = entities.Where(expression);

            }

            if (include is not null)
            {
                foreach (var item in include)
                {
                    entities = entities.Include(item);
                }
            }

            if (tracked)
            {
                entities = entities.AsNoTracking();
            }

            return entities.ToList();
        }

        public bool SaveRange(T entity)
        {
            try
            {
                _db.AddRange(entity);
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public bool RemoveRange(IEnumerable<T> entity)
        {
            try
            {
                _db.RemoveRange(entity);
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public bool AddRange(IEnumerable<T> entity)
        {
            try
            {
                _db.AddRange(entity);
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public T? GetOne(Expression<Func<T, bool>>? expression = null,
            Expression<Func<T, object>>[]? include = null,
            bool tracked = false)
        {
            return Get(expression, include, tracked).FirstOrDefault();
        }

        public bool Commit()
        {
            try
            {
                _db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
