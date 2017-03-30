using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebChat.Models.Im
{
    public class MessageModel
    {
        public string message { get; set; }

        public int dialogueId { get; set; }

        public DateTime SendTime { get; set; }

        public string SenderName { get; set; }
    }
}