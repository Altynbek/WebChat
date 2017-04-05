using System.Collections.Generic;

namespace WebChat.Models.Home
{
    public class GroupListModel
    {
        public GroupListModel()
        {
            Groups = new List<GroupModel>();
        }

        public List<GroupModel> Groups { get; set; }
    }
}