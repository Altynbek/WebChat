using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using WebChat.Classes;

namespace WebChat.Classes.Db.Structure
{
    public class DbUser : IdentityUser
    {
        public DbUser()
        {
            UserDialogs = new List<DbUserDialogue>();
        }

        public virtual ICollection<DbUserDialogue> UserDialogs { get; set; }

        public bool? IsOnline { get; set; }

        public string PhotoUrl { get; set; }

        public string UserId
        {
            get { return Id; }
        }
    }
}