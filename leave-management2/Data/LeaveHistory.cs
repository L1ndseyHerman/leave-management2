using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management2.Data
{
    public class LeaveHistory
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("RequestingEmployeeId")]
        //  This is from the IdentityUser, Employee inherits it:
        public Employee RequestingEmployee { get; set; }
        public string RequestingEmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [ForeignKey("LeaveTypeId")]
        public LeaveType LeaveType { get; set; }
        public int LeaveTypeId { get; set; }
        public DateTime DateRequested { get; set; }
        public DateTime DateActioned { get; set; }
        //  The "?" means null is allowed, could be true, false, or null (pending).
        public bool? Approved { get; set; }
        //  Want to know which other employee approved your leave.
        [ForeignKey("ApprovedById")]
        public Employee ApprovedBy { get; set; }
        public string ApprovedById { get; set; }

    }
}
