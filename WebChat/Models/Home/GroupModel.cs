using System;
using System.Collections.Generic;
using WebChat.Models.Im;

namespace WebChat.Models.Home
{
    public class GroupModel
    {
        public int DialogueId { get; internal set; }

        public string Name { get; internal set; }

        public string PhotoUrl { get; internal set; }

        public DateTime RecentActivityDate { get; internal set; }

        public bool HasNewMessages { get; internal set; }

        public List<string> CompanionsId { get; internal set; }

        public List<MessageModel> Messages { get; internal set; }
    }
}