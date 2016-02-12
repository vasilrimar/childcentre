using ChildCentre.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace ChildCentre.Controllers
{
    public class AboutController : Controller
    {

        //
        // GET: /About/

        public ActionResult Index()
        {
            return View();
        }

    }
}
