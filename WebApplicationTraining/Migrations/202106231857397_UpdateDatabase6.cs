namespace WebApplicationTraining.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDatabase6 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.TrainerCourses", name: "Trainer_Id", newName: "TrainerId");
            RenameIndex(table: "dbo.TrainerCourses", name: "IX_Trainer_Id", newName: "IX_TrainerId");
            DropColumn("dbo.TrainerCourses", "TraineeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TrainerCourses", "TraineeId", c => c.String());
            RenameIndex(table: "dbo.TrainerCourses", name: "IX_TrainerId", newName: "IX_Trainer_Id");
            RenameColumn(table: "dbo.TrainerCourses", name: "TrainerId", newName: "Trainer_Id");
        }
    }
}
