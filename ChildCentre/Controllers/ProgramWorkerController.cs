using ChildCentre.Filters;
using ChildCentre.Models;
using ChildCentre.ViewModels;
using ChildCentre.ViewModels.Repositories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ChildCentre.Controllers
{
    [InitializeSimpleMembership]
    [Authorize(Roles = "ProgramWorker")]
    public class ProgramWorkerController : Controller
    {
        RepoProgram programs = new RepoProgram();
        
        public ActionResult ManageDrafts(int? id)
        {
            int getId = 0;

            if (id != null)
                getId = (int)id;

            if (id != null)
                return View(programs.GetPwDrafts(getId));
            else
                return View(programs.GetPwDrafts(0));
        }

        public ActionResult AddDraft()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddDraft(ProgramDraftUpdate program)
        {
            if (ModelState.IsValid)
            {
                if (!programs.ProgramNameExists(program.Name))
                {                      
                    if (program.Action == "Propose")
                        programs.AddDraft(null, program, DefaultImage());
                    else if (program.Action == "Save")
                    {
                        TempData["isSuccess"] = "display:block";
                        programs.SaveDraft(null, program, DefaultImage());
                        return View(program);
                    }

                    return RedirectToAction("ManageDrafts", "ProgramWorker");
                }
                else ModelState.AddModelError("", "Program name already exists.");
            }

            return View(program);
        }


        public ActionResult EditDraft(int id)
        {
            var draft = programs.GetById(id);
            ProgramDraftUpdate draftViewModel = new ProgramDraftUpdate();
            draftViewModel.Id = draft.Id;
            draftViewModel.Name = draft.Name;
            draftViewModel.Description = draft.Description;
            draftViewModel.pwComments = draft.pwComments;

            return View(draftViewModel);
        }

        [HttpPost]
        public ActionResult EditDraft(ProgramDraftUpdate program)
        {
            if (ModelState.IsValid)
            {
                var a = programs.GetById(program.Id);
                if (!programs.ProgramNameExists(program.Name) || a.Name == program.Name)
                {
                    if (program.Action == "Propose")
                    {
                        programs.AddDraft(program.Id, program, DefaultImage());
                        return RedirectToAction("ManageDrafts", "ProgramWorker");
                    }
                    else if (program.Action == "Save")
                    {
                        programs.SaveDraft(program.Id, program, DefaultImage());
                        TempData["isSuccess"] = "display:block";
                        return View(program);
                    }
                    else return RedirectToAction("ManageDrafts", "ProgramWorker");
                }
                else ModelState.AddModelError("", "Program name already exists.");
            }
            return View(program);
        }

        [HttpPost]
        public void DeleteDraft(int id)
        {
            programs.DeleteDraft(id);
        }

        [HttpPost]
        public void CancelDraft(int id)
        {
            programs.CancelDraft(id);
        }

        public ActionResult TakeAttendance()
        {
            return View(programs.StartAttendance());
        }

        public void ToggleSignIn(string username)
        {
            programs.ToggleSignIn(username);
        }

        public byte[] DefaultImage()
        {
            byte[] picBytes = null;
            var dir = Server.MapPath("/Images");
            var path = System.IO.Path.Combine(dir, "placeholder.png");
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            picBytes = new byte[fs.Length];
            fs.Read(picBytes, 0, System.Convert.ToInt32(fs.Length));
            fs.Close();

            return picBytes;
        }
    }
}
