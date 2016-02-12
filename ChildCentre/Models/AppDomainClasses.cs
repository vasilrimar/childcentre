using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace ChildCentre.Models
{
    public abstract class UserProfile
    {
        public int Id { get; set; }
        public String UserName { get; set; }
        public String LastName { get; set; }
        public String GivenName { get; set; }
        public String Email { get; set; }
        public String PrimaryPhone { get; set; }
        public String SecondaryPhone { get; set; }
        public DateTime LastUpdated { get; set; }
        public virtual Address Address { get; set; }
    }
    public class Address
    {
        public int Id { get; set; }
        public String StreetName { get; set; }
        public String PostalCode { get; set; }
        public String City { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
    public class Guardian : UserProfile    
    {
        public int NumberOfChildren { get; set; }
        public int GuardianType { get; set; }
        public virtual List<Program> Programs { get; set; }
        public virtual List<AttendanceSheet> AttendanceSheets { get; set; }

        public Guardian()
        {
            Programs = new List<Program>();
            AttendanceSheets = new List<AttendanceSheet>();
        }
    }

    public class StaffMember : UserProfile
    {
        public string staffType { get; set; }
    }

    public class Program
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public Status Status { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string DayOffered { get; set; }
        public int ChildLimit { get; set; }
        public virtual List<AttendanceSheet> AttendanceSheets { get; set; }
        public virtual List<Guardian> Guardians { get; set; }
        public String FileName { get; set; }                    
        public Byte[] FileData { get; set; }                    
        public DateTime DateCreated { get; set; }
        public DateTime LastUpdated { get; set; }
        public String pwName { get; set; }
        public String pwComments { get; set; }
        public String supervisorComments { get; set; }
        
        public Program()
        {
            AttendanceSheets = new List<AttendanceSheet>();
        }
    }

    public enum Status
    {
        Editing = 1,
        PendingReview = 2,
        Returned = 3,
        Active = 4,
        Inactive = 5,
    };

    public class AttendanceSheet
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public virtual Program Program { get; set; }
        public virtual List<Guardian> Guardians { get; set; }
        public int TotalRegistered { get; set; }
        public AttendanceSheet()
        {
            Guardians = new List<Guardian>();
        }

    }

}