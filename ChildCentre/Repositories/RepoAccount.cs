using AutoMapper;
using ChildCentre.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebMatrix.WebData;

namespace ChildCentre.ViewModels.Repositories
{
    public class RepoAccount : RepoBase
    {
        public bool UserExists(String username)
        {
            var user = ds.UserProfiles.FirstOrDefault(g => g.UserName == username);
            if (user != null)
                return true;
            else return false;
        }

        public Guardian GetGaurdian()
        {
            var user = ds.Guardians.FirstOrDefault(g => g.UserName == HttpContext.Current.User.Identity.Name);
            return user;
        }

        public ProfileModel GetGuardian(int id)
        {
            var guardian = ds.Guardians.Include("Address").FirstOrDefault(x => x.Id == id);
            return Mapper.Map<ProfileModel>(guardian);
        }

        public void UpdateGuardian(ProfileModel guardian)
        {
            var user = ds.Guardians.Include("Address")
                .SingleOrDefault(u => u.Id == guardian.Id);

            user.GivenName = guardian.GivenName;
            user.LastName = guardian.LastName;
            user.Email = guardian.Email;
            user.PrimaryPhone = guardian.PrimaryPhone;
            user.SecondaryPhone = guardian.SecondaryPhone;
            user.Address = guardian.Address;
            user.LastUpdated = TimeZoneInfo.ConvertTime(DateTime.Now, Global.timeZoneInfo); ;

            ds.Entry(user).CurrentValues.SetValues(user);
            ds.SaveChanges();
        }

        public void DeleteGuardian(string name)
        {
            var guardian = ds.Guardians.FirstOrDefault(s => s.UserName == name);
            var address = ds.Addresses.FirstOrDefault(a => a.UserProfile.Id == guardian.Id);
            if (guardian != null)
            {
                if (address != null)
                    ds.Addresses.Remove(address);
                ds.Guardians.Remove(guardian);
                ds.SaveChanges();
            }
        }

        public ProfileModel GetUserProfile()
        {
            var user = ds.UserProfiles.Include("Address")
                .SingleOrDefault(u => u.UserName == HttpContext.Current.User.Identity.Name);
            
            return Mapper.Map<ProfileModel>(user);
        }

        public void AddUser(RegisterModel model, String accountType, String staffType)
        {
            try
            {
                WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                if (accountType.Equals("guardian"))
                {
                    Roles.AddUserToRole(model.UserName, "Guardian");
                    var newProfile = new Guardian
                    {
                        UserName = model.UserName,
                        GivenName = model.GivenName,
                        LastName = model.LastName,
                        Email = model.Email,
                        PrimaryPhone = model.PrimaryPhone,
                        SecondaryPhone = model.SecondaryPhone,
                        LastUpdated = TimeZoneInfo.ConvertTime(DateTime.Now, Global.timeZoneInfo) ,
                        Address = new Address
                        {
                            StreetName = model.Address.StreetName,
                            City = model.Address.City,
                            PostalCode = model.Address.PostalCode

                        },
                        NumberOfChildren = 1
                    };
                    ds.Guardians.Add(newProfile);
                    ds.SaveChanges();
                }
                else if (accountType.Equals("staff"))
                {
                    if (staffType.Equals("programworker"))
                        Roles.AddUserToRole(model.UserName, "ProgramWorker");
                    else Roles.AddUserToRole(model.UserName, "Supervisor");

                    var newProfile = new StaffMember
                    {
                        UserName = model.UserName,
                        GivenName = model.GivenName,
                        LastName = model.LastName,
                        Email = model.Email,
                        PrimaryPhone = model.PrimaryPhone,
                        SecondaryPhone = model.SecondaryPhone,
                        LastUpdated = TimeZoneInfo.ConvertTime(DateTime.Now, Global.timeZoneInfo) ,
                        Address = new Address
                        {
                            StreetName = model.Address.StreetName,
                            City = model.Address.City,
                            PostalCode = model.Address.PostalCode

                        },
                        staffType = staffType
                    };
                    ds.StaffMembers.Add(newProfile);
                    ds.SaveChanges();
                }
            }
            catch (Exception e)
            {
            }
        }
        
        public void UpdateProfile(ProfileModel updatedInfo)
        {
            var user = ds.UserProfiles.Include("Address")
                .SingleOrDefault(u => u.UserName == WebSecurity.CurrentUserName);

            user.GivenName = updatedInfo.GivenName;
            user.LastName = updatedInfo.LastName;
            user.Email = updatedInfo.Email;
            user.PrimaryPhone = updatedInfo.PrimaryPhone;
            user.SecondaryPhone = updatedInfo.SecondaryPhone;
            user.Address = updatedInfo.Address;
            user.LastUpdated = TimeZoneInfo.ConvertTime(DateTime.Now, Global.timeZoneInfo);

            ds.Entry(user).CurrentValues.SetValues(user);
            ds.SaveChanges();
        }

        public IEnumerable<GuardianList> GetGuardianList()
        {
            var staffList = ds.Guardians.Where(s => s.UserName != WebSecurity.CurrentUserName);
            return Mapper.Map<IEnumerable<GuardianList>>(staffList);
        }

        public IEnumerable<StaffList> GetStaffList()
        {
            var staffList = ds.StaffMembers.Where(s => s.UserName != WebSecurity.CurrentUserName);
            return Mapper.Map<IEnumerable<StaffList>>(staffList);
        }

        public StaffModel GetStaff(int id)
        {
            var staff = ds.StaffMembers.Include("Address").FirstOrDefault(x => x.Id == id);
            return Mapper.Map<StaffModel>(staff);
        }

        public void UpdateStaff(StaffModel staff)
        {
            var user = ds.StaffMembers.Include("Address")
                .SingleOrDefault(u => u.Id == staff.Id);

            user.GivenName = staff.GivenName;
            user.LastName = staff.LastName;
            user.Email = staff.Email;
            user.PrimaryPhone = staff.PrimaryPhone;
            user.SecondaryPhone = staff.SecondaryPhone;
            user.Address = staff.Address;
            user.staffType = staff.StaffType;
            user.LastUpdated = TimeZoneInfo.ConvertTime(DateTime.Now, Global.timeZoneInfo); ;

            ds.Entry(user).CurrentValues.SetValues(user);
            ds.SaveChanges();
        }
        
        public void DeleteStaff(string name)
        {
            var staff = ds.StaffMembers.FirstOrDefault(s => s.UserName == name);
            var address = ds.Addresses.FirstOrDefault(a => a.UserProfile.Id == staff.Id);
            if (staff != null)
            {
                if (address != null)
                    ds.Addresses.Remove(address);
                ds.StaffMembers.Remove(staff);
                ds.SaveChanges();
            }          
        }
              
    }
}