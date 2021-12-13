using leave_management2.Contracts;
using leave_management2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace leave_management2.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        //  New! w the GenericRepository only.
        private readonly DbSet<T> _db;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            //  New too! T = some generic class, aka any class that is passed in is fine.
            _db = _context.Set<T>();
        }

        public async Task Create(T entity)
        {
            await _db.AddAsync(entity);
        }

        public void Delete(T entity)
        {
             _db.Remove(entity);
        }

        //  The expression would be like .Where(q => q.Days > 21)
        //  so FindAll(q => q.Days > 21)
        public async Task<IList<T>> FindAll(
            Expression<Func<T, bool>> expression = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, 
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null
            )
        {
            IQueryable<T> query = _db;

            if (expression != null)
            {
                query = query.Where(expression);
            }

            if (includes != null)
            {
                query = includes(query);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.ToListAsync();

        }

        public async Task<T> Find(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null)
        {
            IQueryable<T> query = _db;

            //  includes would have a list of strings of the table names.
            if (includes != null)
            {
                query = includes(query);
            }

            return await query.FirstOrDefaultAsync(expression);
        }

        public async Task<bool> isExists(Expression<Func<T, bool>> expression = null)
        {
            //  Normally, need to parse an IQueryable to an IList or an IEnumerable for other operations.
            IQueryable<T> query = _db;
            //  The "expression" will be something like q => q.Id == 10
            return await query.AnyAsync(expression);
        }

        public void Update(T entity)
        {
            _db.Update(entity);
        }
    }
}
