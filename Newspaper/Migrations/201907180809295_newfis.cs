namespace Newspaper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newfis : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "fiscalyear_Id", c => c.Int());
            CreateIndex("dbo.Accounts", "fiscalyear_Id");
            AddForeignKey("dbo.Accounts", "fiscalyear_Id", "dbo.FiscalYears", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Accounts", "fiscalyear_Id", "dbo.FiscalYears");
            DropIndex("dbo.Accounts", new[] { "fiscalyear_Id" });
            DropColumn("dbo.Accounts", "fiscalyear_Id");
        }
    }
}
