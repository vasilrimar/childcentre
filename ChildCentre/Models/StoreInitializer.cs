using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebMatrix.WebData;

namespace ChildCentre.Models
{
    //public class StoreInitializer : DropCreateDatabaseAlways<DataContext>
    public class StoreInitializer : DropCreateDatabaseIfModelChanges<DataContext>
    {
        protected override void Seed(DataContext context)
        {
            
            /*
            Program p1 = new Program
            {
                Name = "Infant Program",
                Description = "An infant program offered on ... ",
                StartTime = new DateTime(2014, 3, 4, 24, 0, 0),
                EndTime = new DateTime(2014, 3, 4, 18, 0, 0),
                Status = Status.Active,
                ChildLimit = 10,
                DateCreated = DateTime.Now,
                LastUpdated = DateTime.Now,
                DayOffered = "Wednesday"
            };

            Session ses1 = new Session
            {
                Program = p1
            };
            ses1.Guardians.Add(g1);
            p1.Sessions.Add(ses1);

            context.Programs.Add(p1);

            Program p2 = new Program
            {
                Name = "Building Blocks",
                Description = "An building blocks program offered on ... ",
                StartTime = new DateTime(2014, 3, 4, 24, 0, 0),
                EndTime = new DateTime(2014, 3, 4, 18, 0, 0),
                Status = Status.Active,
                ChildLimit = 10,
                DateCreated = DateTime.Now,
                LastUpdated = DateTime.Now,
                DayOffered = "Thursday"
            };*/
        }
    }
}