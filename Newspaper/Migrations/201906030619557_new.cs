namespace Newspaper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _new : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Prakashanreports", "GroupName_Id", c => c.Int());
            AddColumn("dbo.Prakashanreports", "PrakashanGroup_Id", c => c.Int());
            AddColumn("dbo.Prakashanreports", "Service_Id", c => c.Int());
            AlterColumn("dbo.Prakashanreports", "GroupId", c => c.Int());
            AlterColumn("dbo.Prakashanreports", "NewspaperId", c => c.Int());
            CreateIndex("dbo.Prakashanreports", "GroupName_Id");
            CreateIndex("dbo.Prakashanreports", "PrakashanGroup_Id");
            CreateIndex("dbo.Prakashanreports", "Service_Id");
            AddForeignKey("dbo.Prakashanreports", "GroupName_Id", "dbo.GroupNames", "Id");
            AddForeignKey("dbo.Prakashanreports", "PrakashanGroup_Id", "dbo.PrakashanGroups", "Id");
            AddForeignKey("dbo.Prakashanreports", "Service_Id", "dbo.tblService", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Prakashanreports", "Service_Id", "dbo.tblService");
            DropForeignKey("dbo.Prakashanreports", "PrakashanGroup_Id", "dbo.PrakashanGroups");
            DropForeignKey("dbo.Prakashanreports", "GroupName_Id", "dbo.GroupNames");
            DropIndex("dbo.Prakashanreports", new[] { "Service_Id" });
            DropIndex("dbo.Prakashanreports", new[] { "PrakashanGroup_Id" });
            DropIndex("dbo.Prakashanreports", new[] { "GroupName_Id" });
            AlterColumn("dbo.Prakashanreports", "NewspaperId", c => c.String());
            AlterColumn("dbo.Prakashanreports", "GroupId", c => c.String());
            DropColumn("dbo.Prakashanreports", "Service_Id");
            DropColumn("dbo.Prakashanreports", "PrakashanGroup_Id");
            DropColumn("dbo.Prakashanreports", "GroupName_Id");
        }
    }
}
