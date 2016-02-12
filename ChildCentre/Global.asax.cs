using AutoMapper;
using ChildCentre.Models;
using ChildCentre.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ChildCentre
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            //System.Data.Entity.Database.SetInitializer(new Models.StoreInitializer());
            
            // Automapper

            // Program ViewModel Mapping
            Mapper.CreateMap<Program, ProgramList>();
            Mapper.CreateMap<ProgramList, Program>();
            Mapper.CreateMap<Program, ProgramDraft>();
            Mapper.CreateMap<Program, CurrentProgram>();
            Mapper.CreateMap<CurrentProgram, Program>();

            Mapper.CreateMap<ProgramSupervisor, Program>();
            Mapper.CreateMap<Program, ProgramSupervisor>();

            Mapper.CreateMap<StaffMember, StaffList>();

            // UserProfile and Address ViewModel Mapping
            Mapper.CreateMap<UserProfile, ProfileModel>();
            Mapper.CreateMap<UserProfile, Attendee>();
            Mapper.CreateMap<Address, ProfileModel>();
            Mapper.CreateMap<ProfileModel,Address>();
            Mapper.CreateMap<ProfileModel, UserProfile>();

            Mapper.CreateMap<StaffMember, StaffList>();
            Mapper.CreateMap<StaffList, StaffMember>();

            Mapper.CreateMap<GuardianList, Guardian>();
            Mapper.CreateMap<Guardian, GuardianList>();

            Mapper.CreateMap<StaffModel, Address>();
            Mapper.CreateMap<StaffModel, UserProfile>();

            Mapper.CreateMap<Address, StaffModel>();
            Mapper.CreateMap<UserProfile, StaffModel>();


        }
    }
}