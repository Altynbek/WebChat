using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebChat.Models.Contact
{
    public class ModalContactModel
    {
        public int ContactId { get; set; }

        public string CompanionId { get; set; }

        public string CompanionName { get; set; }

        public string ContactPhotoUrl { get; set; }

        public bool Confirmed { get; set; }

        public bool Selected { get; set; }
    }
}