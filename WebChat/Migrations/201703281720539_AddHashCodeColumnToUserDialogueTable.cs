namespace WebChat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHashCodeColumnToUserDialogueTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DbUserDialogues", "HashCode", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DbUserDialogues", "HashCode");
        }
    }
}
