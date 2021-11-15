using leave_management2.Contracts;
using leave_management2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management2.Repository
{
    public class LeaveTypeRepository : ILeaveTypeRepository
    {
        private readonly ApplicationDbContext _db;

        //  Constructor:
        public LeaveTypeRepository(ApplicationDbContext db)
        {
            //  Alt to "this._db = _db".
            _db = db;
        }

        public bool Create(LeaveType entity)
        {
            _db.LeaveTypes.Add(entity);
            return Save();
        }

        public bool Delete(LeaveType entity)
        {
            _db.LeaveTypes.Remove(entity);
            return Save();
        }

        public ICollection<LeaveType> FindAll()
        {
            //  Gets a List (like ArrayList in Java) of everything in the table.
            //  Wait, why is it "var"? Doesn't C# need to be strongly-typed?
            var leaveTypes = _db.LeaveTypes.ToList();
            return leaveTypes;
        }

        public LeaveType FindById(int id)
        {
            //  "Find()" is like "indexOf()".
            //  Could do "FirstOfDefault()" like at OnShift, but he doesn't tho.
            var leaveType = _db.LeaveTypes.Find(id);
            return leaveType;
        }

        public ICollection<LeaveType> GetEmployeesByLeaveType(int id)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            var changes = _db.SaveChanges();
            //  Should always change 1+ things, check if that's true:
            return changes > 0;
        }

        public bool Update(LeaveType entity)
        {
            _db.LeaveTypes.Update(entity);
            //  Did this save correctly? Return T/F.
            return Save();
        }
    }
}
