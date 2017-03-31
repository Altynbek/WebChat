using System.Collections.Generic;

namespace WebChat.Models.Im
{
    public class DialogueInfoModel
    {
        public string DialogueId { get; internal set; }

        public string Name { get; internal set; }

        public string ShortMessage { get; internal set; }

        public string PhotoUrl { get; internal set; }

        public string RecentActivityDate { get; internal set; }

        public bool IsContactConfirmed { get; internal set; }

        public string FriendsheepInitiator { get; internal set; }

        public string CompanionId { get; internal set; }

        public List<MessageModel> Messages { get; internal set; }
    }
}