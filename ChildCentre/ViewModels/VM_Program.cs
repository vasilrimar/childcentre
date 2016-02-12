using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChildCentre.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChildCentre.ViewModels
{
    public class ReportGraph
    {
        public String date { get; set; }
        public int number { get; set; }

    }

    public class ProgramList 
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public Status Status { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public String DayOffered { get; set; }
        public int ChildLimit { get; set; }

        public bool Registered { get; set; }
    }

    public class ProgramDraft
    {
        public int Id { get; set; }

        [Required(ErrorMessage="Program Name is required")]
        [Display(Name = "Program Name : ")]
        public String Name { get; set; }

        [Required]
        [Display(Name = "Description : ")]
        public String Description { get; set; }

        [Required]
        [Display(Name = "File : ")]
        public String FileName { get; set; }
        public Byte[] FileData { get; set; } 

        public Status Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastUpdated { get; set; }
        public String pwName { get; set; }
        public String pwComments { get; set; }
        public String supervisorComments { get; set; }
    }

    public class ProgramDraftUpdate
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Program Name is required.")]
        [Display(Name = "Program Name : ")]
        public String Name { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [Display(Name = "Description : ")]
        public String Description { get; set; }

        [NotMapped]
        [Display(Name = "Upload Image File : ")]
        public HttpPostedFileBase File { get; set; }

        public String Action { get; set; }
        [Display(Name = "Program Worker Comments : ")]
        public String pwComments { get; set; }
    }

    public class ActiveProgramEdit : Program
    {
        [NotMapped]
        [Display(Name = "Upload Image File : ")]
        public HttpPostedFileBase File { get; set; }
    }

    public class ProgramSupervisor
    {
        public int Id { get; set; }

        public String Name { get; set; }

        [Required(ErrorMessage = "Start time is required.")]
        public string StartTime { get; set; }

        [Required(ErrorMessage = "End time is required.")]
        public string EndTime { get; set; }

        [Required(ErrorMessage = "Day of the week is required.")]
        [Display(Name = "Day of the week offered on : ")]
        public string DayOffered { get; set; }

        [Required(ErrorMessage = "Number of children is required.")]
        [Display(Name = "Child Limit : ")]
        public string ChildLimit { get; set; }
    }

    public class ProgramSupervisorDeny
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter comments explaining your decision.")]
        [Display(Name = "Supervisor Comments :")]
        public String supervisorComments { get; set; }
    }
}