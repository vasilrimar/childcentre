using ChildCentre.Filters;
using ChildCentre.ViewModels;
using ChildCentre.ViewModels.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChildCentre.Controllers
{
    [InitializeSimpleMembership]
    public class HomeController : Controller
    {
        RepoProgram programs = new RepoProgram();

        public ActionResult Index()
        {
            ViewBag.Message = "Homepage";

            var programList = programs.GetAll();

            return View(programList);
        }

    }
}
