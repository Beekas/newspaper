namespace Newspaper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class counterfiscal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "FiscalYear", c => c.String());
            AlterColumn("dbo.Accounts", "BillNo", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Accounts", "BillNo", c => c.String());
            DropColumn("dbo.Accounts", "FiscalYear");
        }
    }
}
