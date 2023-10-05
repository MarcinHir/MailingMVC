namespace MailingAppMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DBUpdate10 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SendedEmails", "Title", c => c.String());
            AlterColumn("dbo.SendedEmails", "Email", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SendedEmails", "Email", c => c.Int(nullable: false));
            AlterColumn("dbo.SendedEmails", "Title", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
