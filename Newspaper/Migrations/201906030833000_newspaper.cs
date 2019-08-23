namespace Newspaper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newspaper : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Prakashanreports", "NepaliDate", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Prakashanreports", "NepaliDate");
        }
    }
}
