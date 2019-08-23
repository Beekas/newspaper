namespace Newspaper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fiscal1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FiscalYears",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NepYear = c.String(),
                        EngYear = c.String(),
                        Status = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        EditedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        EditedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FiscalYears");
        }
    }
}
