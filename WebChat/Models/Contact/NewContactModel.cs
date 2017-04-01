using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebChat.Models.Contact
{
    public class NewContactModel
    {
        public string ContactId { get; set; }

        public string ContactName { get; set; }

        public string PhotoUrl { get; set; }
    }
}