using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChildCentre.ViewModels
{
    public class CurrentProgram
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public String DayOffered { get; set; }
    }
    public class Attendee
    {
        public String UserName { get; set; }
        public String LastName { get; set; }
        public String GivenName { get; set; }
        public bool Attended { get; set; }
    }
    public class AttendanceModel
    {
        public CurrentProgram CurrentProgram { get; set; }
        public IEnumerable<Attendee> AttendanceList { get; set; }

    }
}