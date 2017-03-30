using System;

namespace WebChat.Classes.Db.Structure
{
    public class DbUserContact
    {
        public int Id { get; set; }

        public string OwnerId { get; set; }

        public string ContactId { get; set; }

        public bool Confirmed { get; set; }

        public string FriendsheepInitiator { get; set; }
    }
}