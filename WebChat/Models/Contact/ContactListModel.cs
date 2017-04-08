using System;
using System.Collections.Generic;

namespace WebChat.Models.Contact
{
    public class ContactListModel
    {
        public ContactListModel()
        {
            Contacts = new List<ContactModel>();
        }

        public List<ContactModel> Contacts { get; set; }

        internal object Select()
        {
            throw new NotImplementedException();
        }
    }
}