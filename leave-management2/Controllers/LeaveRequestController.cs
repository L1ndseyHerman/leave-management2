using AutoMapper;
using leave_management2.Contracts;
using leave_management2.Data;
using leave_management2.Data.Migrations;
using leave_management2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management2.Controllers
{
    //  This means you have to be logged in as some role or other to view data, but the role doesn't matter.
    [Authorize]
    public class LeaveRequestController : Controller
    {
        private readonly ILeaveRequestRepository _leaveRequestRepo;
        private readonly ILeaveTypeRepository _leaveTypeRepo;
        private readonly ILeaveAllocationRepository _leaveAllocRepo;
        private readonly IMapper _mapper;
        private readonly UserManager<Employee> _userManager;

        public LeaveRequestController(
            ILeaveRequestRepository leaveRequestRepo,
            ILeaveTypeRepository leaveTypeRepo,
            ILeaveAllocationRepository leaveAllocRepo,
            IMapper mapper,
            UserManager<Employee> userManager
            )
        {
            _leaveRequestRepo = leaveRequestRepo;
            _leaveTypeRepo = leaveTypeRepo;
            _leaveAllocRepo = leaveAllocRepo;
            _mapper = mapper;
            _userManager = userManager;
        }

        //  This is an Index View for administrators only!
        [Authorize(Roles = "Administrator")]
        // GET: LeaveRequestController
        public ActionResult Index()
        {
            var leaveRequests = _leaveRequestRepo.FindAll();
            var leaveRequestsModel = _mapper.Map<List<LeaveRequestVM>>(leaveRequests);
            var model = new AdminLeaveRequestViewVM
            {
                TotalRequests = leaveRequestsModel.Count,
                ApprovedRequests = leaveRequestsModel.Count(q => q.Approved == true),
                //  It's possible for this boolean to be null bec there's a "?" by the property.
                PendingRequests = leaveRequestsModel.Count(q => q.Approved == null),
                RejectedRequests = leaveRequestsModel.Count(q => q.Approved == false),
                LeaveRequests = leaveRequestsModel
            };
            return View(model);
        }

        public ActionResult MyLeave()
        {
            var employee = _userManager.GetUserAsync(User).Result;
            var employeeid = employee.Id;
            var employeeAllocations = _leaveAllocRepo.GetLeaveAllocationsByEmployee(employeeid);
            var employeeRequests = _leaveRequestRepo.GetLeaveRequestsByEmployee(employeeid);

            var employeeAllocationsModel = _mapper.Map<List<LeaveAllocationVM>>(employeeAllocations);
            var employeeRequestsModel = _mapper.Map<List<LeaveRequestVM>>(employeeRequests);

            var model = new EmployeeLeaveRequestViewVM
            {
                LeaveAllocations = employeeAllocationsModel,
                LeaveRequests = employeeRequestsModel
            };

            return View(model);
        }

        // GET: LeaveRequestController/Details/5
        public ActionResult Details(int id)
        {
            var leaveRequest = _leaveRequestRepo.FindById(id);
            var model = _mapper.Map<LeaveRequestVM>(leaveRequest);
            return View(model);
        }

        public ActionResult ApproveRequest(int id)
        {
            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                var leaveRequest = _leaveRequestRepo.FindById(id);
                var employeeid = leaveRequest.RequestingEmployeeId;
                var leaveTypeId = leaveRequest.LeaveTypeId;
                var allocation = _leaveAllocRepo.GetLeaveAllocationsByEmployeeAndType(employeeid, leaveTypeId);

                int daysRequested = (int)(leaveRequest.EndDate - leaveRequest.StartDate).TotalDays;
                //  Remember, there will never be more days requested than are allowed, that logic is checked in a different method.

                //  same as:    allocation.NumberOfDays = allocation.NumberOfDays - daysRequested;
                allocation.NumberOfDays -= daysRequested;

                leaveRequest.Approved = true;
                leaveRequest.ApprovedById = user.Id;
                leaveRequest.DateActioned = DateTime.Now;

                _leaveRequestRepo.Update(leaveRequest);
                _leaveAllocRepo.Update(allocation);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult RejectRequest(int id)
        {
            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                var leaveRequest = _leaveRequestRepo.FindById(id);
                leaveRequest.Approved = false;
                leaveRequest.ApprovedById = user.Id;
                leaveRequest.DateActioned = DateTime.Now;

                _leaveRequestRepo.Update(leaveRequest);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: LeaveRequestController/Create
        public ActionResult Create()
        {
            var leaveTypes = _leaveTypeRepo.FindAll();
            var leaveTypeItems = leaveTypes.Select(q => new SelectListItem
            {
                Text = q.Name,
                Value = q.Id.ToString()
            });

            var model = new CreateLeaveRequestVM
            {
                LeaveTypes = leaveTypeItems
            };

            return View(model);
        }

        // POST: LeaveRequestController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateLeaveRequestVM model)
        {
            try
            {
                var startDate = Convert.ToDateTime(model.StartDate);
                var endDate = Convert.ToDateTime(model.EndDate);
                var leaveTypes = _leaveTypeRepo.FindAll();
                var leaveTypeItems = leaveTypes.Select(q => new SelectListItem
                {
                    Text = q.Name,
                    Value = q.Id.ToString()
                });

                model.LeaveTypes = leaveTypeItems;

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                //  This makes sure the startDate is less than the endDate!
                if (DateTime.Compare(startDate, endDate) > 1)
                {
                    ModelState.AddModelError("", "Start Date cannot be further in the future than the End Date");
                    return View(model);
                }

                var employee = _userManager.GetUserAsync(User).Result;
                var allocation = _leaveAllocRepo.GetLeaveAllocationsByEmployeeAndType(employee.Id, model.LeaveTypeId);
                //  This gets a double, so casting to an int:
                int daysRequested = (int)(endDate.Date - startDate.Date).TotalDays;

                if (daysRequested > allocation.NumberOfDays)
                {
                    ModelState.AddModelError("", "You Do Not Have Sufficient Days For This Request");
                    return View(model);
                }

                var leaveRequestModel = new LeaveRequestVM
                {
                    RequestingEmployeeId = employee.Id,
                    StartDate = startDate,
                    EndDate = endDate,
                    Approved = null,
                    DateRequested = DateTime.Now,
                    DateActioned = DateTime.Now,
                    LeaveTypeId = model.LeaveTypeId,
                    RequestComments = model.RequestComments
                };

                var leaveRequest = _mapper.Map<LeaveRequest>(leaveRequestModel);
                var isSuccess = _leaveRequestRepo.Create(leaveRequest);

                if (!isSuccess)
                {
                    ModelState.AddModelError("", "Something went wrong with submitting your record");
                    return View(model);
                }

                return RedirectToAction(nameof(Index), "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Something went wrong");
                return View(model);
            }
        }

        public ActionResult CancelRequest(int id)
        {
            var leaveRequest = _leaveRequestRepo.FindById(id);
            leaveRequest.Cancelled = true;
            _leaveRequestRepo.Update(leaveRequest);
            return RedirectToAction("MyLeave");
        }

        // GET: LeaveRequestController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LeaveRequestController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LeaveRequestController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LeaveRequestController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
