using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management2.Models
{
    //  VM stands for "ViewModel". It's his naming convention.
    //  This is coppied-n-pasted from LeaveType.cs, but don't always want to just copy-n-paste. Might not want to show all columns on the actual webpage.
    public class DetailsLeaveTypeVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
    }

    //  This is dif, what u would need to type into the form, Id and Date happen automatically.
    public class CreateLeaveTypeVM
    {
        [Required]
        public string Name { get; set; }
    }
}
