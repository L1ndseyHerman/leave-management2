using leave_management2.Contracts;
using leave_management2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//  Need this for more async options:
using Microsoft.EntityFrameworkCore;

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

        //  Added "async", "Task<>", "await", and "Add()" to "AddAsync()" 
        public async Task<bool> Create(LeaveType entity)
        {
            await _db.LeaveTypes.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(LeaveType entity)
        {
            _db.LeaveTypes.Remove(entity);
            return await Save();
        }

        public async Task<ICollection<LeaveType>> FindAll()
        {
            //  Gets a List (like ArrayList in Java) of everything in the table.
            //  Wait, why is it "var"? Doesn't C# need to be strongly-typed?
            var leaveTypes = await _db.LeaveTypes.ToListAsync();
            return leaveTypes;
        }

        public async Task<LeaveType> FindById(int id)
        {
            //  "Find()" is like "indexOf()".
            //  Could do "FirstOfDefault()" like at OnShift, but he doesn't tho.
            var leaveType = await _db.LeaveTypes.FindAsync(id);
            return leaveType;
        }

        public async Task<ICollection<LeaveType>> GetEmployeesByLeaveType(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> isExists(int id)
        {
            //  This would check if there are any rows in the table:
            //var exists = _db.LeaveTypes.Any();

            //  q = a row. Does any row have an id that matches the id parameter passed into the method? Return t/f.
            var exists = await _db.LeaveTypes.AnyAsync(q => q.Id == id);
            return exists;
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            //  Should always change 1+ things, check if that's true:
            return changes > 0;
        }

        public async Task<bool> Update(LeaveType entity)
        {
            _db.LeaveTypes.Update(entity);
            //  Did this save correctly? Return T/F.
            return await Save();
        }
    }
}
