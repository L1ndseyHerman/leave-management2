using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management2.Contracts
{
    //  Was originally just "interface IRepositoryBase", he added the extra stuff:
    //  This will work if u pass any class in here.
    public interface IRepositoryBase<T> where T : class
    {
        //  Means any class can go here, and return everything from the database (SELECT * FROM tablename;).
        ICollection<T> FindAll();
        T FindById(int id);
        //  The "bool" for these return if they were successful or not? So why don't the other ones have that? Bec just getting data, not updating?
        bool Create(T entity);
        bool Update(T entity);
        bool Delete(T entity);
        bool Save();
    }
}
