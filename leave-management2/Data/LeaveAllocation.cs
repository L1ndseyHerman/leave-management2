using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
//  Needed for table joins:
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management2.Data
{
    public class LeaveAllocation
    {
        [Key]
        public int Id { get; set; }
        public int NumberOfDays { get; set; }
        public DateTime DateCreated { get; set; }
        //  This is from the Employee.cs table!
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
        //  Joining on the EmployeeId:
        public string EmployeeId { get; set; }
        [ForeignKey("LeaveTypeId")]
        public LeaveType LeaveType { get; set; }
        public int LeaveTypeId { get; set; }
        //  This is like 2021 ten vacation days, makes sure u don't get 10 more until 2022.
        public int Period { get; set; }
    }
}
