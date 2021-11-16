using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management2.Models
{
    //  VM stands for "ViewModel". It's his naming convention.
    //  This is coppied-n-pasted from LeaveType.cs, but don't always want to just copy-n-paste. Might not want to show all columns on the actual webpage.
    public class LeaveTypeVM
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        //  Normally, it displays the variable name in the webpage. This adds a space btw "DateCreated".
        [Display(Name="Date Created")]
        //  The "?" here is bec u want to wait to set it, will be null at the form, and then u set it to the current date later.
        public DateTime? DateCreated { get; set; }
    }
}
