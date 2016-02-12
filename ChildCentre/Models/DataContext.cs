using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ChildCentre.Models
{
    public class DataContext : DbContext
    {
        public DataContext() : base("name=DefaultConnection") { }
        
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Guardian> Guardians { get; set; }
        public DbSet<StaffMember> StaffMembers { get; set; }
        public DbSet<Program> Programs { get; set; }
        public DbSet<AttendanceSheet> AttendanceSheets { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProfile>()
            .HasRequired(u => u.Address)
            .WithRequiredPrincipal(u => u.UserProfile);

            base.OnModelCreating(modelBuilder);
        }
    }


}