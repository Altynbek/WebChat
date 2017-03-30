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
    }
}