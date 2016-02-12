using AutoMapper;
using ChildCentre.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace ChildCentre.ViewModels.Repositories
{
    public class RepoProgram : RepoBase 
    {
        public Program GetById(int? id)
        {
            return ds.Programs
                .Include("AttendanceSheets")
                .Include("Guardians")
                .FirstOrDefault(i => i.Id == id);
        }

        public bool ProgramNameExists(String name)
        {
            var program = ds.Programs.SingleOrDefault(p => p.Name == name);
            if (program != null)
                return true;
            else return false;
        }
        
        public IEnumerable<ProgramList> GetAll(Status status = Status.Active)
        {
            var programs = ds.Programs.Where(p => p.Status == status).OrderBy(p=> p.StartTime.Hour);
            return Mapper.Map<IEnumerable<ProgramList>>(programs);
        }

        public IEnumerable<ProgramList> GetAllForCurrentUser()
        {
            var guardian = ds.Guardians.Include("Programs").SingleOrDefault(g => g.UserName == WebSecurity.CurrentUserName);
            var programs = ds.Programs
                .Where(p => p.Status == Status.Active).OrderBy(p => p.StartTime.Hour);

            var program_list = Mapper.Map<IEnumerable<ProgramList>>(programs);
            
            foreach (var p in program_list)
            {
                if (guardian.Programs.SingleOrDefault(pr => pr.Id == p.Id) != null)
                    p.Registered = true;
                else
                    p.Registered = false;
            }

            return program_list;
        }

        public IEnumerable<ProgramDraft> GetPwDrafts(int filterBy)
        {
            var programs = (IEnumerable<Program>)null;

            if (filterBy == 1)
            {
                programs = ds.Programs
                    .Where(p => (p.Status == Status.Active) &&
                        p.pwName == HttpContext.Current.User.Identity.Name).OrderByDescending(p => p.LastUpdated);
            }
            else if (filterBy == 2)
            {
                programs = ds.Programs
                    .Where(p => (p.Status == Status.Editing) &&
                        p.pwName == HttpContext.Current.User.Identity.Name).OrderByDescending(p => p.LastUpdated);
            }
            else if (filterBy == 3)
            {
                programs = ds.Programs
                    .Where(p => (p.Status == Status.PendingReview) &&
                        p.pwName == HttpContext.Current.User.Identity.Name).OrderByDescending(p => p.LastUpdated);
            }
            else if (filterBy == 4)
            {
                programs = ds.Programs
                    .Where(p => (p.Status == Status.PendingReview) &&
                        p.pwName == HttpContext.Current.User.Identity.Name).OrderByDescending(p => p.LastUpdated);
            }
            else
            {
                    programs = ds.Programs
                        .Where(p => (p.Status == Status.Active || p.Status == Status.Editing
                            || p.Status == Status.Returned || p.Status == Status.PendingReview) &&
                            p.pwName == HttpContext.Current.User.Identity.Name).OrderByDescending(p => p.LastUpdated);
            }

            return Mapper.Map<IEnumerable<ProgramDraft>>(programs);
        }

        public IEnumerable<Program> GetPrograms()
        {
            var programs = ds.Programs.Where(p=> p.Status == Status.Active 
                || p.Status == Status.Inactive || p.Status == Status.PendingReview).OrderByDescending(p => p.DateCreated);

            return Mapper.Map<IEnumerable<Program>>(programs);
        }

        public void AddDraft(int? id, ProgramDraftUpdate update, byte[] picBytes)
        {
            byte[] imageBytes = null;
            String type;
            if (update.File == null)
            {
                imageBytes = new byte[picBytes.Length];
                imageBytes = picBytes;
                type = ".png";
            }
            else
            {
                imageBytes = new byte[update.File.ContentLength];
                update.File.InputStream.Read(imageBytes, 0, update.File.ContentLength);
                type = update.File.ContentType;
            }
            if (id != null)
            {
                var saveDraft = ds.Programs.SingleOrDefault(d => d.Id == update.Id);
                saveDraft.Name = update.Name;
                saveDraft.pwComments = update.pwComments;
                saveDraft.Description = update.Description;
                saveDraft.LastUpdated = TimeZoneInfo.ConvertTime(DateTime.Now, Global.timeZoneInfo);
                saveDraft.Status = Status.PendingReview;
                if (saveDraft.FileData == null && update.File == null)
                {
                    imageBytes = new byte[picBytes.Length];
                    imageBytes = picBytes;
                    type = ".png";
                    saveDraft.FileData = imageBytes;
                    saveDraft.FileName = type;
                }
                ds.Entry(saveDraft).CurrentValues.SetValues(saveDraft);
                ds.SaveChanges();
            }
            else
            {
                var program = new Program
                {
                    Name = update.Name,
                    ChildLimit = 0,
                    Description = update.Description,
                    pwComments = update.pwComments,
                    FileName = type,
                    FileData = imageBytes,
                    DateCreated = TimeZoneInfo.ConvertTime(DateTime.Now, Global.timeZoneInfo),
                    Status = Status.PendingReview,
                    LastUpdated = TimeZoneInfo.ConvertTime(DateTime.Now, Global.timeZoneInfo),
                    EndTime = TimeZoneInfo.ConvertTime(DateTime.Now, Global.timeZoneInfo),
                    StartTime = TimeZoneInfo.ConvertTime(DateTime.Now, Global.timeZoneInfo),
                    pwName = HttpContext.Current.User.Identity.Name
                };
                ds.Programs.Add(program);
            } 
            
            ds.SaveChanges();
        }

        public void SaveDraft(int? id,ProgramDraftUpdate update, byte[] picBytes)
        {
            if (id != null)
            {
                byte[] imageBytes = null;
                String type;

                var saveDraft = ds.Programs.SingleOrDefault(d => d.Id == update.Id);
                saveDraft.Name = update.Name;
                saveDraft.pwComments = update.pwComments;
                saveDraft.Description = update.Description;
                saveDraft.LastUpdated = TimeZoneInfo.ConvertTime(DateTime.Now, Global.timeZoneInfo);
                if (saveDraft.FileData == null && update.File == null)
                {
                    saveDraft.FileData = picBytes;
                    saveDraft.FileName = "image/png";
                }
                else if (update.File != null)
                {
                    imageBytes = new byte[update.File.ContentLength];
                    update.File.InputStream.Read(imageBytes, 0, update.File.ContentLength);
                    type = update.File.ContentType;
                    saveDraft.FileData = imageBytes;
                    saveDraft.FileName = type;
                }
                ds.Entry(saveDraft).CurrentValues.SetValues(saveDraft);
                ds.SaveChanges();
            }
            else
            {
                byte[] imageBytes = null;
                String type;
                if (update.File == null)
                {
                    imageBytes = new byte[picBytes.Length];
                    imageBytes = picBytes;
                    type = "image/png";
                }
                else 
                {
                    imageBytes = new byte[update.File.ContentLength];
                    update.File.InputStream.Read(imageBytes, 0, update.File.ContentLength);
                    type = update.File.ContentType;
                }
                var program = new Program
                {
                    Name = update.Name,
                    ChildLimit = 0,
                    Description = update.Description,
                    pwComments = update.pwComments,
                    FileName = "image/png",
                    FileData = imageBytes,
                    DateCreated = TimeZoneInfo.ConvertTime(DateTime.Now, Global.timeZoneInfo),
                    Status = Status.Editing,
                    LastUpdated = TimeZoneInfo.ConvertTime(DateTime.Now, Global.timeZoneInfo),
                    EndTime = TimeZoneInfo.ConvertTime(DateTime.Now, Global.timeZoneInfo),
                    StartTime = TimeZoneInfo.ConvertTime(DateTime.Now, Global.timeZoneInfo),
                    pwName = HttpContext.Current.User.Identity.Name
                };
                ds.Programs.Add(program);
                ds.SaveChanges(); 
            }
        }

        public void CancelDraft(int id)
        {
            var program = ds.Programs.SingleOrDefault(p => p.Id == id);
            program.Status = Status.Editing;
            ds.SaveChanges();
        }
       
        public Program GetProgram(int id)
        {
            var program = ds.Programs.FirstOrDefault(p => p.Id == id);
            return program;
        }

        public Program CheckTime(ProgramSupervisor model)
        {
            int startHour = Convert.ToInt32(model.StartTime);
            int endHour = Convert.ToInt32(model.EndTime);

            DateTime sTime = new DateTime(2013, 04, 12, startHour, 0, 0);
            DateTime eTime = new DateTime(2013, 04, 12, endHour, 0, 0);
            
            var currentProgram = ds.Programs
                .FirstOrDefault(cp => cp.DayOffered == model.DayOffered
                    && cp.StartTime.Hour < eTime.Hour && cp.EndTime.Hour > sTime.Hour);

            if (currentProgram != null)
                if (model.StartTime.Equals(currentProgram.StartTime.Hour.ToString())
                    && model.EndTime.Equals(currentProgram.EndTime.Hour.ToString())
                    && currentProgram.Name == model.Name)
                    return null;

            return currentProgram;
        }
        
        public void AcceptDraft(ProgramSupervisor model)
        {
            var saveDraft = ds.Programs.Include("AttendanceSheets")
                .SingleOrDefault(d => d.Id == model.Id);

            int startHour = Convert.ToInt32(model.StartTime);
            int endHour = Convert.ToInt32(model.EndTime);

            DateTime sTime = new DateTime(2013, 04, 12, startHour, 0, 0);
            DateTime eTime = new DateTime(2013, 04, 12, endHour, 0, 0);

            if (saveDraft != null)
            {
                saveDraft.DayOffered = model.DayOffered;
                saveDraft.StartTime = sTime;
                saveDraft.EndTime = eTime;
                saveDraft.ChildLimit = Convert.ToInt16(model.ChildLimit);
                saveDraft.LastUpdated = TimeZoneInfo.ConvertTime(DateTime.Now, Global.timeZoneInfo);
                saveDraft.Status = Status.Active;
                ds.SaveChanges();
            }
        }

        public void DenyDraft(ProgramSupervisorDeny model)
        {
            var saveDraft = ds.Programs.SingleOrDefault(d => d.Id == model.Id);
            if (saveDraft != null)
            {
                saveDraft.Status = Status.Returned;
                saveDraft.LastUpdated = TimeZoneInfo.ConvertTime(DateTime.Now, Global.timeZoneInfo);
                saveDraft.supervisorComments = model.supervisorComments;
                ds.Entry(saveDraft).CurrentValues.SetValues(saveDraft);
                ds.SaveChanges();
            }
        }

        public void SaveProgram(Program model)
        {
            var saveDraft = ds.Programs.Include("AttendanceSheets")
                .SingleOrDefault(d => d.Id == model.Id);

            if (saveDraft != null)
            {
                saveDraft.FileData = model.FileData;
                saveDraft.FileName = model.FileName;
                saveDraft.Name = model.Name;
                saveDraft.Description = model.Description;
                saveDraft.DayOffered = model.DayOffered;
                saveDraft.StartTime = model.StartTime;
                saveDraft.EndTime = model.EndTime;
                saveDraft.ChildLimit = Convert.ToInt16(model.ChildLimit);
                saveDraft.LastUpdated = TimeZoneInfo.ConvertTime(DateTime.Now, Global.timeZoneInfo);
                saveDraft.Status = Status.Active;
                ds.SaveChanges();
            }
        }

        public void Deactivate(int id)
        {
            var saveDraft = ds.Programs.SingleOrDefault(d => d.Id == id);
            if (saveDraft != null)
            {
                saveDraft.Status = Status.Inactive;
                saveDraft.LastUpdated = TimeZoneInfo.ConvertTime(DateTime.Now, Global.timeZoneInfo);
                ds.Entry(saveDraft).CurrentValues.SetValues(saveDraft);
                ds.SaveChanges();
            }
        }

        public void Activate(int id)
        {
            var saveDraft = ds.Programs.SingleOrDefault(d => d.Id == id);
            if (saveDraft != null)
            {
                saveDraft.Status = Status.Active;
                saveDraft.LastUpdated = TimeZoneInfo.ConvertTime(DateTime.Now, Global.timeZoneInfo);
                ds.Entry(saveDraft).CurrentValues.SetValues(saveDraft);
                ds.SaveChanges();
            }
        }

        public void DeleteDraft(int id)
        {
            var draftToDelete = ds.Programs
                .SingleOrDefault(p => p.Id == id);

           var attendances = ds.AttendanceSheets
               .Include("Program")
               .Where(d => d.Program.Id == id);

           if (attendances != null)
               foreach (var a in attendances)
                   ds.AttendanceSheets.Remove(a);

            ds.SaveChanges();
            ds.Programs.Remove(draftToDelete);
            ds.SaveChanges();
        }

        public Guardian GetGaurdian()
        {
            var user = ds.Guardians.FirstOrDefault(g => g.UserName == HttpContext.Current.User.Identity.Name);
            return user;
        }

        public Guardian getGuardian(string username)
        {
            return ds.Guardians.SingleOrDefault(g => g.UserName == username);
        }

        public int GetCurrentProgramID()
        {
            DateTime Date = TimeZoneInfo.ConvertTime(DateTime.Now, Global.timeZoneInfo);
            var day = Date.DayOfWeek.ToString();
            var currentTime = Date.TimeOfDay.Hours;
            var curMinutes = Date.TimeOfDay.Minutes;

            if (Date.Hour > 18 || Date.Hour < 12)
                day = "All";

            //bool invertResult = end < start;

            var currentProgram = ds.Programs
                .FirstOrDefault(cp => cp.DayOffered == day 
                    && Date.Hour >=  cp.StartTime.Hour &&  Date.Hour <= cp.EndTime.Hour
                    && cp.Status == Status.Active);

            if (currentProgram != null)
                return currentProgram.Id;
            else return -1;
        }

        public AttendanceModel StartAttendance()
        {
            AttendanceModel model = new AttendanceModel();
            model.AttendanceList = null;
            model.CurrentProgram = null;

            int id = GetCurrentProgramID();
            var currentProgram = ds.Programs
                .Include("AttendanceSheets")
                .Include("Guardians")
                .SingleOrDefault(p => p.Id == id);

            AttendanceSheet newAttendance = new AttendanceSheet();
            int now = TimeZoneInfo.ConvertTime(DateTime.Now, Global.timeZoneInfo).Day;
            if (currentProgram != null)
            {
                var sheet = ds.AttendanceSheets
                    .Include("Program")
                    .Include("Guardians")
                    .SingleOrDefault(a => a.Program.Id == currentProgram.Id
                        && a.Date.Day.Equals(now));

                var attendees = Mapper.Map<IEnumerable<Attendee>>(currentProgram.Guardians);
                if (sheet == null)
                {
                    newAttendance.Program = currentProgram;
                    newAttendance.Date = TimeZoneInfo.ConvertTime(DateTime.Now, Global.timeZoneInfo);
                    newAttendance.TotalRegistered = currentProgram.Guardians.Count;
                    currentProgram.AttendanceSheets.Add(newAttendance);
                    ds.SaveChanges();
                    
                    model.CurrentProgram = Mapper.Map<CurrentProgram>(currentProgram);
                    model.AttendanceList = attendees;
                }
                else
                {
                    model.CurrentProgram = Mapper.Map<CurrentProgram>(currentProgram);
                    foreach (var a in attendees)
                    {
                        if (sheet.Guardians.Contains(getGuardian(a.UserName)))
                            a.Attended = true;
                        else a.Attended = false;
                    }
                    model.AttendanceList = attendees;
                }
            }
            return model;
        }

        public void ToggleSignIn(string username)
        {
            int id = GetCurrentProgramID();
            int now = TimeZoneInfo.ConvertTime(DateTime.Now, Global.timeZoneInfo).Day;
            var curAttendance = ds.AttendanceSheets
                .Include("Guardians")
                .Include("Program")
                .SingleOrDefault(p => p.Program.Id == id
                    && p.Date.Day.Equals(now));

            if (curAttendance != null)
            {
                var guardian = getGuardian(username);
                if (!curAttendance.Guardians.Contains(guardian))
                    curAttendance.Guardians.Add(guardian);
                else curAttendance.Guardians.Remove(guardian);

                ds.SaveChanges();
            }
        }
        
        public IEnumerable<ProgramList> ViewRegisteredPrograms()
        {
            var programs = ds.Programs
                .Where(s => s.Status != Status.Inactive && 
                    s.Guardians.FirstOrDefault(g => g.UserName == HttpContext.Current.User.Identity.Name) != null)
                .AsEnumerable();

            return Mapper.Map<IEnumerable<ProgramList>>(programs);
        }

        public void RegisterForProgram(int programId, int guardianId) 
        {
            var guardian = ds.Guardians.Include("Address").SingleOrDefault(g => g.Id == guardianId);
            var program = ds.Programs.Include("Guardians").SingleOrDefault(p => p.Id == programId);

            if (program != null)
                if (!program.Guardians.Contains(guardian))
                    program.Guardians.Add(guardian);
            ds.SaveChanges();
        }

        public void UnregisterFromProgram(int id, int guardianId)
        {
            var guardian = ds.Guardians.Include("Address").SingleOrDefault(g => g.Id == guardianId);
            var program = ds.Programs.Include("Guardians").SingleOrDefault(p => p.Id == id);

            if (program != null)
                if (program.Guardians.Contains(guardian))
                    ds.Entry(program).Entity.Guardians.Remove(guardian);

            ds.SaveChanges();
        }

        public IEnumerable<AttendanceSheet> GetAllAttendancesForProgram(int id)
        {
            var attendanceSheets = ds.AttendanceSheets
                .Include("Program")
                .Include("Guardians")
                .Where(p => p.Program.Id == id).OrderBy(p => p.Date);

            return attendanceSheets;
        }
    }
}