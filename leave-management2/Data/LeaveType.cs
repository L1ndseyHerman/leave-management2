using System;
using System.Collections.Generic;
//  Needed for the things in [].
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management2.Data
{
    public class LeaveType
    {
        //  These things in [] say something abt the property below it, like to not allow nulls or whatevs.
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int DefaultDays { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
