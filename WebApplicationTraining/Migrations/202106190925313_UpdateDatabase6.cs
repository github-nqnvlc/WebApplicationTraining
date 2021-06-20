namespace WebApplicationTraining.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDatabase6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "WorkingPlace", c => c.String());
            AddColumn("dbo.AspNetUsers", "Name", c => c.String());
            AddColumn("dbo.AspNetUsers", "Age", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "Phone", c => c.Int(nullable: false));
            AddColumn("dbo.TrainerTopics", "Course", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TrainerTopics", "Course");
            DropColumn("dbo.AspNetUsers", "Phone");
            DropColumn("dbo.AspNetUsers", "Age");
            DropColumn("dbo.AspNetUsers", "Name");
            DropColumn("dbo.AspNetUsers", "WorkingPlace");
        }
    }
}
