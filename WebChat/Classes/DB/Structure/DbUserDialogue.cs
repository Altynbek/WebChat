using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebChat.Models;

namespace WebChat.Classes.Db.Structure
{
    public class DbUserDialogue
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [ForeignKey("Dialogue")]
        public int DialogueId { get; set; }

        public string DialogueName { get; set; }

        public int HashCode { get; set; }

        public DbUser User { get; set; }

        public DbDialogue Dialogue { get; set; }
    }
}