using ChildCentre.ViewModels.Repositories;
using ChildCentre.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ChildCentre.Filters;

namespace ChildCentre.Controllers
{
    [InitializeSimpleMembership]
    [Authorize(Roles = "Guardian")]
    public class GuardianController : Controller
    {
        RepoProgram programs = new RepoProgram();
        RepoAccount accounts = new RepoAccount();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register(int id)
        {
            if (id > 0) programs.RegisterForProgram(id, accounts.GetGaurdian().Id);
            return RedirectToAction("Index", "Programs");
        }

        public ActionResult Unregister(int id)
        {
            if (id > 0) programs.UnregisterFromProgram(id, accounts.GetGaurdian().Id);
            return RedirectToAction("ViewRegisteredPrograms", "Guardian");
        }

        public ActionResult UnregisterProgram(int id)
        {
            if (id > 0) programs.UnregisterFromProgram(id, accounts.GetGaurdian().Id);
            return RedirectToAction("Index", "Programs");

        }

        public ActionResult ViewRegisteredPrograms()
        {
            var registeredPrograms = programs.ViewRegisteredPrograms();

            return View(registeredPrograms);
        }

    }
}
