using AutoMapper;
using ChildCentre.Models;
using ChildCentre.ViewModels;
using ChildCentre.ViewModels.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChildCentre.Controllers
{
    public class ProgramsController : Controller
    {
        RepoProgram programs = new RepoProgram();
        //
        // GET: /Programs/
        
        public ActionResult Index()
        {

            if (User.IsInRole("Guardian") )
            {
                return View(programs.GetAllForCurrentUser());
            }
            return View(programs.GetAll()); 
        }

        [HttpPost]
        public ActionResult Index(string searchFieldText = "")
        {
            var query = "SELECT * FROM dbo.Programs WHERE name LIKE '%" + searchFieldText + "%'";
            
            using (var context = new DataContext())
            {
                var result = context.Database.SqlQuery<Program>(query, new object[] { }).AsEnumerable();

                return View(Mapper.Map<IEnumerable<ProgramList>>(result));  
            }
        }


        public ActionResult Image(int? id)
        {
            var image = programs.GetById(id);

            if (image.FileData == null)
                return HttpNotFound();

            else return File(image.FileData, image.FileName);
        }

    }
}
