using ChildCentre.Filters;
using ChildCentre.Models;
using ChildCentre.ViewModels;
using ChildCentre.ViewModels.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace ChildCentre.Controllers
{
    [InitializeSimpleMembership]
    [Authorize(Roles = "Supervisor")]
    public class SupervisorController : Controller
    {
        RepoProgram programs = new RepoProgram();
        RepoAccount accounts = new RepoAccount();

        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult StaffList()
        {
            return View(accounts.GetStaffList());
        }

        public ActionResult AddStaff()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddStaff(RegisterModel model, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                if (!accounts.UserExists(model.UserName))
                {
                    accounts.AddUser(model, "staff", form["staffType"]);
                    return RedirectToAction("StaffList");
                }
                else ModelState.AddModelError("", "Username already in use. Please choose a different one.");
            }
            return View();
        }

        public ActionResult EditStaff(int id)
        {
            return View(accounts.GetStaff(id));
        }

        [HttpPost]
        public ActionResult EditStaff(StaffModel model)
        {
            if (ModelState.IsValid)
            {
                accounts.UpdateStaff(model);
                TempData["isSuccess"] = "display:block";
            }
            return View(model);
        }

        public ActionResult DeleteStaff(string name)
        {
            if (name != null)
            {
                if (Roles.IsUserInRole(name, "ProgramWorker"))
                    Roles.RemoveUserFromRole(name, "ProgramWorker");
                else
                    Roles.RemoveUserFromRole(name, "Supervisor");
                var membership = (SimpleMembershipProvider)Membership.Provider;
                bool wasDeleted = membership.DeleteAccount(name);
                wasDeleted = membership.DeleteUser(name, true);
                accounts.DeleteStaff(name);
            }
            return RedirectToAction("StaffList");
        }

        public ActionResult GuardianList()
        {
            return View(accounts.GetGuardianList());
        }

        public ActionResult AddGuardian()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddGuardian(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if (!accounts.UserExists(model.UserName))
                {
                    accounts.AddUser(model, "guardian", "");
                    return RedirectToAction("GuardianList");
                }
                else ModelState.AddModelError("", "Username already in use. Please choose a different one.");
            }
            return View();
        }

        public ActionResult EditGuardian(int id)
        {
            return View(accounts.GetGuardian(id));
        }

        [HttpPost]
        public ActionResult EditGuardian(ProfileModel model)
        {
            if (ModelState.IsValid)
            {
                accounts.UpdateGuardian(model);
                TempData["isSuccess"] = "display:block";
            }
            return View(model);
        }

        public ActionResult DeleteGuardian(string name)
        {
            if (name != null)
            {
                Roles.RemoveUserFromRole(name, "Guardian");
                var membership = (SimpleMembershipProvider)Membership.Provider;
                bool wasDeleted = membership.DeleteAccount(name);
                wasDeleted = membership.DeleteUser(name, true);
                accounts.DeleteGuardian(name);
            }
            return RedirectToAction("GuardianList");
        }

        public ActionResult ManagePrograms()
        {
            var programList = programs.GetPrograms();
            return View(programList);
        }

        public ActionResult AcceptDraft(int id)
        {
            var p = programs.GetProgram(id);
            ViewBag.Id = id;
            ViewBag.Name = p.Name;
            return View();
        }

        [HttpPost]
        public ActionResult AcceptDraft(int id, ProgramSupervisor model)
        {
            ViewBag.Id = id;

            if (ModelState.IsValid)
            {
                if (Convert.ToInt32(model.StartTime) < Convert.ToInt32(model.EndTime))
                {
                    var p = programs.CheckTime(model);

                    if (p == null)
                    {
                        var program = programs.GetById(id);

                        model.Id = id;
                        programs.AcceptDraft(model);
                        return RedirectToAction("ManagePrograms", "Supervisor");
                    }
                    else
                    {
                        var program = programs.GetById(p.Id);

                        ModelState.AddModelError("", "Program '"
                        + program.Name + "' already running on " + program.DayOffered
                        + " between the time " + program.StartTime.ToShortTimeString()
                        + " - " + program.EndTime.ToShortTimeString() + ". Please select a different timeslot");
                    }
                }
                else ModelState.AddModelError("", "End time must be later than start time.");
            }
            return View(model);
        }

        public ActionResult DenyDraft(int id)
        {
            var p = programs.GetProgram(id);
            ViewBag.Id = id;
            ViewBag.Name = p.Name;
            return View();
        }

        [HttpPost]
        public ActionResult DenyDraft(int id, ProgramSupervisorDeny model)
        {
            ViewBag.Id = id;

            if (ModelState.IsValid)
            {
                model.Id = id;
                programs.DenyDraft(model);

                return RedirectToAction("ManagePrograms", "Supervisor");
            }
            return View(model);
        }

        public ActionResult Deactivate(int id)
        {
            programs.Deactivate(id);
            return RedirectToAction("ManagePrograms", "Supervisor");
        }

        public ActionResult Activate(int id)
        {
            programs.Activate(id);
            return RedirectToAction("ManagePrograms", "Supervisor");
        }

        [HttpPost]
        public void DeleteDraft(int id)
        {
            programs.DeleteDraft(id);
        }

        public ActionResult EditProgram(int id)
        {
            var program = programs.GetProgram(id);
            ViewBag.Id = id;
            ViewBag.StartTimes = new SelectList(
                        new[]
                        {
                            new {Value = 12,Text="12:00 PM"},
                            new {Value = 13,Text="1:00 PM"},
                            new {Value = 14,Text="2:00 PM"},
                            new {Value = 15,Text="3:00 PM"},
                            new {Value = 16,Text="4:00 PM"},
                            new {Value = 17,Text="5:00 PM"},
                        }, "Value", "Text", program.StartTime.Hour.ToString());
            ViewBag.EndTimes = new SelectList(
                        new[]
                        {
                            new {Value = 13,Text="1:00 PM"}, 
                            new {Value = 14,Text="2:00 PM"},
                            new {Value = 15,Text="3:00 PM"},
                            new {Value = 16,Text="4:00 PM"},
                            new {Value = 17,Text="5:00 PM"},
                            new {Value = 18,Text="6:00 PM"},
                        }, "Value", "Text", program.EndTime.Hour.ToString());
            
            ViewBag.Days = new SelectList(
                        new[]
                        {
                            new {Value = "Monday",Text="Monday"},
                            new {Value = "Tuesday",Text="Tuesday"},
                            new {Value = "Wednesday",Text="Wednesday"},
                            new {Value = "Thursday",Text="Thursday"},
                            new {Value = "Friday",Text="Friday"},
                        }, "Value", "Text", program.DayOffered.ToString());

            return View(program);
        }

        [HttpPost]
        public ActionResult EditProgram(int id, Program model, FormCollection form)
        {
            ViewBag.Id = id;
            if (ModelState.IsValid)
            {
                var pa = programs.GetById(id);
                if (pa == null)
                    return RedirectToAction("ManagePrograms");

                int startHour = Convert.ToInt32(form["StartTimes"]);
                int endHour = Convert.ToInt32(form["EndTimes"]);

                DateTime sTime = new DateTime(2013, 04, 12, startHour, 0, 0);
                DateTime eTime = new DateTime(2013, 04, 12, endHour, 0, 0);
                pa.DayOffered = form["Days"];
                pa.Description = model.Description;
                pa.StartTime = sTime;
                pa.EndTime = eTime;

                ViewBag.StartTimes = new SelectList(new[]
                    {
                        new {Value = 12,Text="12:00 PM"},
                        new {Value = 13,Text="1:00 PM"},
                        new {Value = 14,Text="2:00 PM"},
                        new {Value = 15,Text="3:00 PM"},
                        new {Value = 16,Text="4:00 PM"},
                        new {Value = 17,Text="5:00 PM"},
                    }, "Value", "Text", pa.StartTime.Hour.ToString());
                ViewBag.EndTimes = new SelectList(new[]
                    {
                        new {Value = 13,Text="1:00 PM"},
                        new {Value = 14,Text="2:00 PM"},
                        new {Value = 15,Text="3:00 PM"},
                        new {Value = 16,Text="4:00 PM"},
                        new {Value = 17,Text="5:00 PM"},
                        new {Value = 18,Text="6:00 PM"},
                    }, "Value", "Text", pa.EndTime.Hour.ToString());

                ViewBag.Days = new SelectList(new[]
                    {
                        new {Value = "Monday",Text="Monday"},
                        new {Value = "Tuesday",Text="Tuesday"},
                        new {Value = "Wednesday",Text="Wednesday"},
                        new {Value = "Thursday",Text="Thursday"},
                        new {Value = "Friday",Text="Friday"},
                    }, "Value", "Text", pa.DayOffered.ToString());

                if (!programs.ProgramNameExists(model.Name) || model.Name == pa.Name)
                {
                    pa.Name = model.Name;
                    var a = AutoMapper.Mapper.Map<ProgramSupervisor>(pa);
                    a.StartTime = sTime.Hour.ToString();
                    a.EndTime = eTime.Hour.ToString();
                    if (startHour < endHour)
                    {
                        var p = programs.CheckTime(a);
                        if (p == null)
                        {
                            var program = programs.GetById(id);

                            model.Id = id;
                            programs.SaveProgram(pa);
                            TempData["isSuccess"] = "display:block";
                            return View(pa);
                        }
                        else
                        {
                            var program = programs.GetById(p.Id);

                            ModelState.AddModelError("", "Program '"
                            + program.Name + "' is already running on " + program.DayOffered
                            + " between the time " + program.StartTime.ToShortTimeString()
                            + " - " + program.EndTime.ToShortTimeString() + ". Please select a different timeslot.");
                        }
                    }
                    else ModelState.AddModelError("", "End time must be later than start time.");
                }
                else ModelState.AddModelError("", "Program name already exists.");
            }
            return View(model);
        }
        
        public ActionResult ReportList()
        {
            return View(programs.GetAll());
        }

        public ActionResult ProgramReport(int id)
        {
            return View(Tuple.Create(programs.GetAllAttendancesForProgram(id), programs.GetById(id)));
        }

    }
}
