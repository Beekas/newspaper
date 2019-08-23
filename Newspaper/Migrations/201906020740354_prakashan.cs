namespace Newspaper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class prakashan : DbMigration
    {
        public override void Up()
        {
           
            CreateTable(
                "dbo.GroupNames",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AgentName = c.String(),
                        Pan = c.String(),
                        GP_Quantity = c.String(),
                        RN_Quantity = c.String(),
                        MUNA_Quantity = c.String(),
                        Address = c.String(),
                        State = c.String(),
                        District = c.String(),
                        Phone = c.String(),
                        Email = c.String(),
                        URL = c.String(),
                        GroupId = c.Int(),
                        PrakashanGroup_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PrakashanGroups", t => t.PrakashanGroup_Id)
                .Index(t => t.PrakashanGroup_Id);
            
            CreateTable(
                "dbo.PrakashanGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GroupId = c.String(),
                        GroupName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Prakashanreports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GroupId = c.String(),
                        Date = c.DateTime(nullable: false),
                        NewspaperId = c.String(),
                        Total = c.Int(nullable: false),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
           
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GroupNames", "PrakashanGroup_Id", "dbo.PrakashanGroups");
       
            DropIndex("dbo.GroupNames", new[] { "PrakashanGroup_Id" });
           
            DropTable("dbo.Prakashanreports");
          
            DropTable("dbo.PrakashanGroups");
            DropTable("dbo.GroupNames");
           
        }
    }
}
