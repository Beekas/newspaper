namespace Newspaper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class serviceimg : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblService", "Images", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblService", "Images");
        }
    }
}
