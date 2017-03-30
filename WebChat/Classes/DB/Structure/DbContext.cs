using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;


namespace WebChat.Classes.Db.Structure
{
    public class DbContext : IdentityDbContext<DbUser>
    {
        public DbContext() : base("WebChatDB") { }

        public DbSet<DbDialogue> Dialogs { get; set; }

        public DbSet<DbMessage> Messages { get; set; }

        public DbSet<DbUserDialogue> UserDialogues { get; set; }

        public DbSet<DbUserContact> UserContacts { get; set; }

        //public DbSet<DbClaim> UserClaims { get; set; }

        public static DbContext Create()
        {
            return new DbContext();
        }
    }
}