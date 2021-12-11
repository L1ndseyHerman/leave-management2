using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace leave_management2.Contracts
{
    //  Was originally just "interface IRepositoryBase", he added the extra stuff:
    //  This will work if u pass any class in here.
    public interface IRepositoryBase<T> where T : class
    {
        //  Means any class can go here, and return everything from the database (SELECT * FROM tablename;).
        //  Adding "Task<>" makes in async somehow?!
        Task<ICollection<T>> FindAll();
        Task<T> FindById(int id);
        //  Checks to see if that id exists in the database or not.
        //  Just added this in vid22, so now need to update other things to make this work.
        Task<bool> isExists(int id);
        //  The "bool" for these return if they were successful or not? So why don't the other ones have that? Bec just getting data, not updating?
        Task<bool> Create(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(T entity);
        Task<bool> Save();
    }

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
            List<string> includes = null
        );
        Task<T> Find(Expression<Func<T, bool>> expression, List<string> includes = null);
        Task<bool> isExists(Expression<Func<T, bool>> expression = null);
        Task Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
