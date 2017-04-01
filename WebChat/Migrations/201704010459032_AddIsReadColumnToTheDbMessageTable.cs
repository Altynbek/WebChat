namespace WebChat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsReadColumnToTheDbMessageTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DbMessages", "IsReaded", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DbMessages", "IsReaded");
        }
    }
}
