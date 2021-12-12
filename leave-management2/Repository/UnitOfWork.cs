using leave_management2.Contracts;
using leave_management2.Data;
using leave_management2.Data.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management2.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IGenericRepository<LeaveType> _leaveTypes;
        private IGenericRepository<LeaveRequest> _leaveRequests;
        private IGenericRepository<LeaveAllocation> _leaveAllocations;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        //  Ternary operator!
        //public IGenericRepository<LeaveType> LeaveTypes => _leaveTypes == null ? _leaveTypes : new GenericRepository<LeaveType>(_context);
        public IGenericRepository<LeaveType> LeaveTypes => _leaveTypes ??= new GenericRepository<LeaveType>(_context);
        public IGenericRepository<LeaveRequest> LeaveRequests => _leaveRequests ??= new GenericRepository<LeaveRequest>(_context);
        public IGenericRepository<LeaveAllocation> LeaveAllocations => _leaveAllocations ??= new GenericRepository<LeaveAllocation>(_context);

        //  For Garbage Collection:
        public void Dispose()
        {
            Dispose(true);
            //  GC = Garbage Collection.
            GC.SuppressFinalize(this);
        }

        //  Also for Garbage Collection:
        private void Dispose(bool dispose)
        {
            if (dispose)
            {
                _context.Dispose();
            }
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
