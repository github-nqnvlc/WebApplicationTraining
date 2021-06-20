namespace WebApplicationTraining.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDatabase2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CourseCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CourseCategoryName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CategoryId = c.Int(nullable: false),
                        TopicId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Topics", t => t.TopicId, cascadeDelete: true)
                .Index(t => t.CategoryId)
                .Index(t => t.TopicId);
            
            CreateTable(
                "dbo.Topics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Trainees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        Email = c.String(),
                        FullName = c.String(),
                        Phone = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.Trainers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(),
                        WorkingPlace = c.String(),
                        Phone = c.String(),
                        TopicId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Topics", t => t.TopicId, cascadeDelete: true)
                .Index(t => t.TopicId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Trainers", "TopicId", "dbo.Topics");
            DropForeignKey("dbo.Trainees", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Courses", "TopicId", "dbo.Topics");
            DropForeignKey("dbo.Courses", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Trainers", new[] { "TopicId" });
            DropIndex("dbo.Trainees", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Courses", new[] { "TopicId" });
            DropIndex("dbo.Courses", new[] { "CategoryId" });
            DropTable("dbo.Trainers");
            DropTable("dbo.Trainees");
            DropTable("dbo.Topics");
            DropTable("dbo.Courses");
            DropTable("dbo.CourseCategories");
            DropTable("dbo.Categories");
        }
    }
}
