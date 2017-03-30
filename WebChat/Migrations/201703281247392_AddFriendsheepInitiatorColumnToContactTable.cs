namespace WebChat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFriendsheepInitiatorColumnToContactTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DbUserContacts", "FriendsheepInitiator", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DbUserContacts", "FriendsheepInitiator");
        }
    }
}
