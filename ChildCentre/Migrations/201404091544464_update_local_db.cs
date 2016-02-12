namespace ChildCentre.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_local_db : DbMigration
    {
        public override void Up()
        {
           // DropForeignKey("dbo.Sessions", "Program_Id", "dbo.Programs");
           // DropForeignKey("dbo.AttendanceSheets", "Id", "dbo.Sessions");
            //DropForeignKey("dbo.SessionGuardians", "Session_Id", "dbo.Sessions");
            //DropForeignKey("dbo.SessionGuardians", "Guardian_Id", "dbo.UserProfiles");
           // DropIndex("dbo.Sessions", new[] { "Program_Id" });
           // DropIndex("dbo.AttendanceSheets", new[] { "Id" });
            //DropIndex("dbo.SessionGuardians", new[] { "Session_Id" });
           // DropIndex("dbo.SessionGuardians", new[] { "Guardian_Id" });
            CreateTable(
                "dbo.ProgramGuardians",
                c => new
                    {
                        Program_Id = c.Int(nullable: false),
                        Guardian_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Program_Id, t.Guardian_Id })
                .ForeignKey("dbo.Programs", t => t.Program_Id, cascadeDelete: true)
                .ForeignKey("dbo.UserProfiles", t => t.Guardian_Id, cascadeDelete: true)
                .Index(t => t.Program_Id)
                .Index(t => t.Guardian_Id);
            
            AddColumn("dbo.AttendanceSheets", "TotalRegistered", c => c.Int(nullable: false));
            AddColumn("dbo.AttendanceSheets", "Program_Id", c => c.Int());
            AlterColumn("dbo.AttendanceSheets", "Id", c => c.Int(nullable: false, identity: true));
            AddForeignKey("dbo.AttendanceSheets", "Program_Id", "dbo.Programs", "Id");
            CreateIndex("dbo.AttendanceSheets", "Program_Id");
            //DropTable("dbo.Sessions");
            //DropTable("dbo.SessionGuardians");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SessionGuardians",
                c => new
                    {
                        Session_Id = c.Int(nullable: false),
                        Guardian_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Session_Id, t.Guardian_Id });
            
            CreateTable(
                "dbo.Sessions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Program_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropIndex("dbo.ProgramGuardians", new[] { "Guardian_Id" });
            DropIndex("dbo.ProgramGuardians", new[] { "Program_Id" });
            DropIndex("dbo.AttendanceSheets", new[] { "Program_Id" });
            DropForeignKey("dbo.ProgramGuardians", "Guardian_Id", "dbo.UserProfiles");
            DropForeignKey("dbo.ProgramGuardians", "Program_Id", "dbo.Programs");
            DropForeignKey("dbo.AttendanceSheets", "Program_Id", "dbo.Programs");
            AlterColumn("dbo.AttendanceSheets", "Id", c => c.Int(nullable: false));
            DropColumn("dbo.AttendanceSheets", "Program_Id");
            DropColumn("dbo.AttendanceSheets", "TotalRegistered");
            DropTable("dbo.ProgramGuardians");
            CreateIndex("dbo.SessionGuardians", "Guardian_Id");
            CreateIndex("dbo.SessionGuardians", "Session_Id");
            CreateIndex("dbo.AttendanceSheets", "Id");
            CreateIndex("dbo.Sessions", "Program_Id");
            AddForeignKey("dbo.SessionGuardians", "Guardian_Id", "dbo.UserProfiles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SessionGuardians", "Session_Id", "dbo.Sessions", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AttendanceSheets", "Id", "dbo.Sessions", "Id");
            AddForeignKey("dbo.Sessions", "Program_Id", "dbo.Programs", "Id");
        }
    }
}
