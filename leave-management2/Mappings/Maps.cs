using AutoMapper;
using leave_management2.Data;
using leave_management2.Data.Migrations;
using leave_management2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management2.Mappings
{
    public class Maps : Profile
    {
        public Maps()
        {
            //  Goes in 1-direction: LeaveType to DetailsLeaveTypeVM, not the other way. ReverseMap() makes it go both ways!
            CreateMap<LeaveType, LeaveTypeVM>().ReverseMap();
            CreateMap<LeaveHistory, LeaveHistoryVM>().ReverseMap();
            CreateMap<LeaveAllocation, LeaveAllocationVM>().ReverseMap();
            CreateMap<LeaveAllocation, EditLeaveAllocationVM>().ReverseMap();
            CreateMap<Employee, EmployeeVM>().ReverseMap();
        }
       
    }
}
