using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebChat.Models;

namespace WebChat.Classes.Db.Structure
{
    public class DbMessage
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Dialogue")]
        public int DialogueId { get; set; }

        [ForeignKey("User")]
        public string CreatorId { get; set; }

        public string Text { get; set; }

        public DateTime SendingDate { get; set; }

        public bool? IsEdited { get; set; }

        public DateTime? LastEditDate { get; set; }

        public DbDialogue Dialogue { get; set; }

        public DbUser User { get; set; }
    }
}