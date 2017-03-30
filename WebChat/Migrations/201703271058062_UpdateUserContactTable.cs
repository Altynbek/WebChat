namespace WebChat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUserContactTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DbUserContacts", "Confirmed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DbUserContacts", "Confirmed");
        }
    }
}
