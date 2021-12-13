using AutoMapper;
using leave_management2.Contracts;
using leave_management2.Data;
using leave_management2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management2.Controllers
{
    //  This keyword prevents un-logged-in users from being able to access this url route!
    //  Can specify which role(s) are able to access here too:
    [Authorize(Roles = "Administrator")]
    public class LeaveTypesController : Controller
    {
        //  U added this, most methods came from the read/write option:
        private readonly ILeaveTypeRepository _repo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LeaveTypesController(ILeaveTypeRepository repo, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: LeaveTypesController

        //  More async changes here:
        //  Moved .ToList() from the "leavetypes" line to the "model" line.
        public async Task<ActionResult> Index()
        {
            //var leavetypes = await _repo.FindAll();
            var leavetypes = await _unitOfWork.LeaveTypes.FindAll();
            var model = _mapper.Map<List<LeaveType>, List<LeaveTypeVM>>(leavetypes.ToList());
            return View(model);
        }

        // GET: LeaveTypesController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            //var isExists = await _repo.isExists(id);
            var isExists = await _unitOfWork.LeaveTypes.isExists(q => q.Id == id);
            //  If this row does not exist in the table:
            if (!isExists)
            {
                return NotFound();
            }
            //var leavetype = await _repo.FindById(id);
            var leavetype = await _unitOfWork.LeaveTypes.Find(q => q.Id == id);
            var model = _mapper.Map<LeaveTypeVM>(leavetype);
            return View(model);
        }

        // GET: LeaveTypesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LeaveTypesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(LeaveTypeVM model)
        {
            try
            {
                //  Darn, I'm not sure what all these checks are doing. Why wouldn't the ModelState be valid?
                if(!ModelState.IsValid)
                {
                    return View(model);
                }
                var leaveType = _mapper.Map<LeaveType>(model);
                leaveType.DateCreated = DateTime.Now;

                //var isSuccess = await _repo.Create(leaveType);
                await _unitOfWork.LeaveTypes.Create(leaveType);
                await _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                //  This shows on the screen, lets the user know something is wrong, like couldn't connect to the database, or whatevs.
                ModelState.AddModelError("", "Something Went Wrong...");
                return View(model);
            }
        }

        // GET: LeaveTypesController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            //var isExists = await _repo.isExists(id);
            var isExists = await _unitOfWork.LeaveTypes.isExists(q => q.Id == id);

            if (!isExists)
            {
                return NotFound();
            }

            //var leavetype = _repo.FindById(id);
            var leavetype = await _unitOfWork.LeaveTypes.Find(q => q.Id == id);
            var model = _mapper.Map<LeaveTypeVM>(leavetype);
            return View(model);
        }

        // POST: LeaveTypesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(LeaveTypeVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var leaveType = _mapper.Map<LeaveType>(model);
                //var isSuccess = await _repo.Update(leaveType);
                //if (!isSuccess)
                //{
                //ModelState.AddModelError("", "Something Went Wrong...");
                //return View(model);
                //}
                _unitOfWork.LeaveTypes.Update(leaveType);
                await _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Something Went Wrong...");
                return View(model);
            }
        }

        // POST: LeaveTypesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        //  Only need the model so that it has another parameter, otherwise C# doesn't know if it should call this Delete method or the other one.
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                //var leavetype = await _repo.FindById(id);
                var leavetype = await _unitOfWork.LeaveTypes.Find(expression: q => q.Id == id);
                if (leavetype == null)
                {
                    return NotFound();
                }
                _unitOfWork.LeaveTypes.Delete(leavetype);
                await _unitOfWork.Save();
            }
            catch
            {
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
