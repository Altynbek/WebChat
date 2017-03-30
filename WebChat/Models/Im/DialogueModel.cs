namespace WebChat.Models.Im
{
    public class DialogueInfoModel
    {
        public string DialogueId { get; set; }         // from userdialogue

        public string Name { get; set; }                // if (isMulty) -> dbDialogue, else from dbUserDialogue

        public string ShortMessage { get; set; }       // from message

        public string PhotoUrl { get; set; }           // if(isMulty)->dbDialogue, else from aspnetUser

        public string RecentActivityDate { get; set; }  // from dbDialogue

        public bool IsContactConfirmed { get; set; }    // from dbUserContact

        public string FriendsheepInitiator { get; set; } // from dbUserContact

        public string CompanionId { get; set; }
    }
}