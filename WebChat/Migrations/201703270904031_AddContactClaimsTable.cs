namespace WebChat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddContactClaimsTable : DbMigration
    {
        public override void Up()
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
        
        public override void Down()
        {
            DropTable("dbo.DbClaims");
        }
    }
}
