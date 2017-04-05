using System;
using System.Collections.Generic;

namespace WebChat.Models.Im
{
    public class GroupDialogueInfoModel
    {
        public int DialogueId { get; internal set; }

        public string Name { get; internal set; }

        public string PhotoUrl { get; internal set; }

        public DateTime RecentActivityDate { get; internal set; }

        public List<string> Companions { get; internal set; }

        public List<MessageModel> Messages { get; internal set; }
    }
}