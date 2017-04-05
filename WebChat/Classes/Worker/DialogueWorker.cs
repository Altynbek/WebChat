using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebChat.Classes.Db.Structure;
using WebChat.Classes.DB.Repositories;
using WebChat.Models.Contact;
using WebChat.Models.Im;

namespace WebChat.Classes.Worker
{
    public class DialogueWorker : IDisposable
    {
        private readonly ContactRepository _contactRepository = null;
        private readonly UserDialogueRepository _userDialogueRepository = null;
        private readonly UserRepository _userRepository = null;
        private readonly DialogueRepository _dialogueRepository = null;

        public DialogueWorker(DbContext context)
        {
            _contactRepository = new ContactRepository(context);
            _userDialogueRepository = new UserDialogueRepository(context);
            _userRepository = new UserRepository(context);
            _dialogueRepository = new DialogueRepository(context);
        }

        public DialogueInfoModel GetDialogueInfo(int contactId)
        {
            var dialogueInfo = new DialogueInfoModel();
            var dbContact = _contactRepository.GetById(contactId);
            int hashcode = GetDialogueHashCode(dbContact.OwnerId, dbContact.ContactId);

            dialogueInfo.PhotoUrl = dbContact.Confirmed ? 
                _userRepository.GetById(dbContact.ContactId).PhotoUrl ?? "/Content/Images/avatar-default.png" : 
                "/Content/Images/avatar-unknown.png";
            dialogueInfo.CompanionId = dbContact.ContactId;
            dialogueInfo.IsContactConfirmed = dbContact.Confirmed;
            dialogueInfo.FriendsheepInitiator = dbContact.FriendsheepInitiator;
            dialogueInfo.Name = _userRepository.GetById(dbContact.ContactId)?.UserName;

            if (!dbContact.Confirmed)
            {    
                return dialogueInfo;
            }
            return dialogueInfo;
        }

        
        public int CreatePersonalDialogue(string firstUserId, string secondUserId)
        {
            var dialogueId = CreateDbDialogue();
            CreateDbUserDialogue(firstUserId, secondUserId, dialogueId);
            return dialogueId;
        }

        private int CreateDbDialogue()
        {
            var currentDate = DateTime.Now;
            var dbDialogue = new DbDialogue()
            {
                CreateDate = currentDate,
                IsMultyUser = false,
                RecentActivityDate = currentDate,
            };
            int createdDialogueId = -1;
            _dialogueRepository.Insert(dbDialogue, out createdDialogueId);
            return createdDialogueId;
        }

        private void CreateDbUserDialogue(string firstUserId, string secondUserId, int dialogueId)
        {
            var user1 = _userRepository.GetById(firstUserId);
            var user2 = _userRepository.GetById(secondUserId);
            int hashCode = GetDialogueHashCode(firstUserId, secondUserId);

            var dbUserDialogue1 = new DbUserDialogue()
            {
                DialogueId = dialogueId,
                UserId = user1.Id,
                DialogueName = user1.UserName,
                HashCode = hashCode,
            };

            var dbUserDialogue2 = new DbUserDialogue()
            {
                DialogueId = dialogueId,
                UserId = user2.Id,
                DialogueName = user2.UserName,
                HashCode = hashCode,
            };
            _userDialogueRepository.Insert(dbUserDialogue1);
            _userDialogueRepository.Insert(dbUserDialogue2);
        }

        public int? GetDialogueIdByHashCode(int hashCode)
        {
            var userDialogue = _userDialogueRepository.SearchFor(x => x.HashCode == hashCode).FirstOrDefault();
            if(userDialogue != null)
            {
                return userDialogue.DialogueId;
            }
            return null;
        }

        public void Dispose()
        {
            _contactRepository.Dispose();
            _dialogueRepository.Dispose();
            _userRepository.Dispose();
        }

        public static int GetDialogueHashCode(string userId1, string userId2)
        {
            var result = userId1.GetHashCode() + userId2.GetHashCode();
            return result;
        }
    }
}