namespace WebChat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropDbClaimsTable : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.DbClaims");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.DbClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreateDate = c.DateTime(nullable: false),
                        SenderId = c.String(),
                        RecipientId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
