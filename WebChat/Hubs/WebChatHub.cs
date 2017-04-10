using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace WebChat.Hubs
{
    public class User
    {
        public string Id { get; set; }

        public string ConnectionId { get; set; }
    }


    public class WebChatHub : Hub
    {
        static List<User> Users = new List<User>();

        public void UpdatePersonalDialogue(string companionId, string message, string senderId)
        {
            Clients.All.UpdatePersonalDialogue(companionId, message, senderId);
        }

        public void UpdateGroupDialogue(string dialogueId, string message, string senderId)
        {
            Clients.All.UpdateGroupDialogue(dialogueId, message, senderId);
        }

        public void Connect(string userId)
        {
            var id = Context.ConnectionId;
            if (!Users.Any(x => x.ConnectionId == id))
            {
                Users.Add(new User { ConnectionId = id, Id = userId });
                Clients.Caller.onConnected(id, userId, Users);
                Clients.AllExcept(id).onNewUserConnected(id, userId);
            }
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var item = Users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (item != null)
            {
                Users.Remove(item);
                var id = Context.ConnectionId;
                Clients.All.onUserDisconnected(id, item.Id);
            }

            return base.OnDisconnected(stopCalled);
        }
    }
}