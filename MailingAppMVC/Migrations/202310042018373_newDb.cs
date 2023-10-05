namespace MailingAppMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SendedEmails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SenderName = c.String(),
                        Title = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Email = c.Int(nullable: false),
                        SendedDate = c.DateTime(nullable: false),
                        Body = c.String(),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SendedEmails", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.SendedEmails", new[] { "UserId" });
            DropTable("dbo.SendedEmails");
        }
    }
}
