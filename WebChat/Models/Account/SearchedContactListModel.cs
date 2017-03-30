using System.Collections.Generic;

namespace WebChat.Models.Account
{
    public class SearchedContactListModel
    {
        public SearchedContactListModel()
        {
            Contacts = new List<SearchedContactModel>();
        }

        public List<SearchedContactModel> Contacts { get; set; }
    }
}