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
        private readonly IMapper _mapper;
        private readonly UserManager<Employee> _userManager;

        public LeaveRequestController(ILeaveRequestRepository leaveRequestRepo, ILeaveTypeRepository leaveTypeRepo, IMapper mapper, UserManager<Employee> userManager)
        {
            _leaveRequestRepo = leaveRequestRepo;
            _leaveTypeRepo = leaveTypeRepo;
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

        // GET: LeaveRequestController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //[Keyless]
        // GET: LeaveRequestController/Create
        //[NotMapped]
        public ActionResult Create()
        {
            var leaveTypes = _leaveTypeRepo.FindAll();
            var leaveTypeItems = leaveTypes.Select(q => new SelectListItem
            //var leaveTypeItems = leaveTypes.Select(q => new IEnumerable<LeaveRequest>
            {
                //Id = q.Id,
                //Key = q.Id,
                Text = q.Name,
                Value = q.Id.ToString()
                //Text = q.Id.ToString(),
                //Value = q.Name
            });

            //  My code to fix his id-less problem:
            //var leaveTypeItemsWithId = leaveTypes;
            /*foreach (var item in leaveTypes)
            {
                item.Id = leaveTypes.LeaveTypeId;
            }*/
            //var leaveTypeIds = leaveTypes.Select(q => q.Id);

            var model = new CreateLeaveRequestVM
            {
                LeaveTypes = leaveTypeItems
                //LeaveTypes.Text = leaveTypeItems.Text
            };
            //HasNoKey();

            return View(model);
        }

        // POST: LeaveRequestController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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
