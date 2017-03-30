using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebChat.Classes.Db.Structure
{
    public class DbDialogue
    {

        public DbDialogue()
        {
            UserDialogs = new List<DbUserDialogue>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsMultyUser { get; set; }

        public string PhotoUrl { get; set; }

        public DateTime RecentActivityDate { get; set; }

        public DateTime CreateDate { get; set; }

        public virtual ICollection<DbUserDialogue> UserDialogs { get; set; }
    }
}