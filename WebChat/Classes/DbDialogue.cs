using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebChat.Classes
{
    public class DbDialogue
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ShortMessage { get; set; }

        public string PhotoUrl { get; set; }

        public string RecentActivityDate { get; set; }
    }
}