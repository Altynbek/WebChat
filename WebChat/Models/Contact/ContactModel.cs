using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebChat.Models.Contact
{
    public class ContactModel
    {
        public int Id { get; set; }

        public string ContactId { get; set; }

        public string ContactName { get; set; }

        public string ContactPhotoUrl { get; set; }

        public bool Confirmed { get; set; }

        public string FriendsheepInitiator { get; set; }

        public bool HasNewMessages { get; set; }
    }
}