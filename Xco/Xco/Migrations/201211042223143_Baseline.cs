namespace Xco.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Baseline : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Links",
                c => new
                    {
                        LinkId = c.Int(nullable: false, identity: true),
                        ShortenedUrl = c.String(),
                        OriginalUrl = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedByEmail = c.String(),
                        IsVanityUrl = c.Boolean(nullable: false),
                        ActivationDate = c.DateTime(),
                        DeactivationDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.LinkId);
            
            CreateTable(
                "dbo.Visits",
                c => new
                    {
                        VisitId = c.Int(nullable: false, identity: true),
                        LinkId = c.Int(nullable: false),
                        Stamp = c.DateTime(nullable: false),
                        Location = c.Geography(),
                    })
                .PrimaryKey(t => t.VisitId)
                .ForeignKey("dbo.Links", t => t.LinkId, cascadeDelete: true)
                .Index(t => t.LinkId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Visits", new[] { "LinkId" });
            DropForeignKey("dbo.Visits", "LinkId", "dbo.Links");
            DropTable("dbo.Visits");
            DropTable("dbo.Links");
        }
    }
}
