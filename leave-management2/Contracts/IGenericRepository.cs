using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace leave_management2.Contracts
{
    //  FINALLY!! He's making a new file for this class instead of putting it in the same file as another one!
    public interface IGenericRepository<T> where T : class
    {
        //  "Expression" is the data type for a lambda expression.
        //  Lambda expressions like: q => q.Id == 20
        //  q is "T", boolean is the bool.
        //  If the boolean is not present, put default of null.

        //  The second param could be for like: q => q.OrderByDescending(q => q.Id)

        //  3rd param for like including 1 table n not the other or whatevs.
        Task<IList<T>> FindAll(
            Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null
        //  Replace all of these with the above line:
        //List<string> includes = null
        );
        Task<T> Find(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null);
        Task<bool> isExists(Expression<Func<T, bool>> expression = null);
        Task Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
