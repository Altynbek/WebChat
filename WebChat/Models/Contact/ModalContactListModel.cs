using System.Collections.Generic;
using System.Web;

namespace WebChat.Models.Contact
{
    public class ModalContactListModel
    {
        public ModalContactListModel()
        {
            Contacts = new List<ModalContactModel>();
        }

        public List<ModalContactModel> Contacts { get; set; }

        public string GroupName { get; set; }

        public HttpPostedFileBase GroupIcon { get; set; }
    }
}