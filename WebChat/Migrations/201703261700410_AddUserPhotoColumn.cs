namespace WebChat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserPhotoColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "PhotoUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "PhotoUrl");
        }
    }
}
