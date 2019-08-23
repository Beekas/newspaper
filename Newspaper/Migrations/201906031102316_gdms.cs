namespace Newspaper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gdms : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Prakashanreports", "Service_Id", "dbo.tblService");
            DropIndex("dbo.Prakashanreports", new[] { "Service_Id" });
            AddColumn("dbo.Prakashanreports", "gp_Total", c => c.Int(nullable: false));
            AddColumn("dbo.Prakashanreports", "rn_Total", c => c.Int(nullable: false));
            DropColumn("dbo.Prakashanreports", "Total");
            DropColumn("dbo.Prakashanreports", "NewspaperId");
            DropColumn("dbo.Prakashanreports", "Service_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Prakashanreports", "Service_Id", c => c.Int());
            AddColumn("dbo.Prakashanreports", "NewspaperId", c => c.Int());
            AddColumn("dbo.Prakashanreports", "Total", c => c.Int(nullable: false));
            DropColumn("dbo.Prakashanreports", "rn_Total");
            DropColumn("dbo.Prakashanreports", "gp_Total");
            CreateIndex("dbo.Prakashanreports", "Service_Id");
            AddForeignKey("dbo.Prakashanreports", "Service_Id", "dbo.tblService", "Id");
        }
    }
}
