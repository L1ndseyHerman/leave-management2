using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management2.Models
{
    public class LeaveAllocationVM
    {
        public int Id { get; set; }
        public int NumberOfDays { get; set; }
        public DateTime DateCreated { get; set; }
        public int Period { get; set; }

        public EmployeeVM Employee { get; set; }
        //  Joining on the EmployeeId:
        public string EmployeeId { get; set; }

        public LeaveTypeVM LeaveType { get; set; }
        public int LeaveTypeId { get; set; }

        //  Below is a drop-down-list of all employees in the database!
        public IEnumerable<SelectListItem> Employees { get; set; }
        public IEnumerable<SelectListItem> LeaveTypes { get; set; }
    }

    //  Aww, not a second class in the same file :(
    public class CreateLeaveAllocationVM
    {
        public int NumberUpdated { get; set; }
        public List<LeaveTypeVM> LeaveTypes { get; set; }
    }
}
