using System;
using System.Collections.Generic;
using System.Linq;
using WebChat.Classes.Db.Structure;
using WebChat.Classes.DB.Repositories;
using WebChat.Models.Im;

namespace WebChat.Classes.Worker
{
    public class MessageWorker : IDisposable
    {
        MessageRepository _messageRepository = null;
        UserRepository _userRepository = null;

        public MessageWorker(DbContext context)
        {
            _messageRepository = new MessageRepository(context);
            _userRepository = new UserRepository(context);
        }

        public void SaveMessage(DbMessage message)
        {
            _messageRepository.Insert(message);
        }

        public List<MessageModel> GetMessages(int dialogueId, int offset = 0, int count = 50)
        {
            var messages = new List<MessageModel>();
            var dbMessages = _messageRepository.SearchFor(x=>x.DialogueId == dialogueId).OrderByDescending(x => x.SendingDate).Skip(offset).Take(count).ToList();

            var usersId = dbMessages.Select(x => x.CreatorId).Distinct().ToList();
            var users = _userRepository.SearchFor(x => usersId.Contains(x.Id)).ToList();

            foreach(var msg in dbMessages)
            {
                string userName = users.Single(x => x.Id == msg.CreatorId).UserName;
                messages.Add(new MessageModel() { dialogueId = dialogueId, message = msg.Text, SenderName = userName, SendTime = msg.SendingDate });
            }
            return messages;
        }

        public void Dispose()
        {
            _messageRepository.Dispose();
            _userRepository.Dispose();
        }

        public int MarkMessagesAsReaded(int dialogueId, string recipientId)
        {
            return _messageRepository.MarkMessagesAsReaded(dialogueId, recipientId);
        }
    }
}