namespace Newspaper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dscountamount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "DiscountAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Accounts", "DiscountAmount");
        }
    }
}
