namespace WebApplicationTraining.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDatabase5 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TrainerCourses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TraineeId = c.String(),
                        CourseId = c.Int(nullable: false),
                        Trainer_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.Trainer_Id)
                .Index(t => t.CourseId)
                .Index(t => t.Trainer_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TrainerCourses", "Trainer_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.TrainerCourses", "CourseId", "dbo.Courses");
            DropIndex("dbo.TrainerCourses", new[] { "Trainer_Id" });
            DropIndex("dbo.TrainerCourses", new[] { "CourseId" });
            DropTable("dbo.TrainerCourses");
        }
    }
}
