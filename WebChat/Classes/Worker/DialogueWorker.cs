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

            var dbUserDialogue = _userDialogueRepository
                .SearchFor( x => x.HashCode == hashcode && x.UserId == dbContact.OwnerId)
                .SingleOrDefault();

            dialogueInfo.CompanionId = dbContact.ContactId;
            dialogueInfo.IsContactConfirmed = dbContact.Confirmed;
            dialogueInfo.FriendsheepInitiator = dbContact.FriendsheepInitiator;
            dialogueInfo.Name = _userRepository.GetById(dbContact.ContactId)?.UserName;

            if (!dbContact.Confirmed)
            {    
                return dialogueInfo;
            }

            //if(dbUserDialogue == null)
            //{
            //}


            //dialogueInfo.FriendsheepInitiator = dbContact.FriendsheepInitiator;
            
            //// если он не подтвержден, то подтверждаем его или ждем
            //dialogueInfo.IsContactConfirmed = dbContact.Confirmed;
            //dialogueInfo.Name = dbUserDialogue?.DialogueName;
            //dialogueInfo.CompanionId = dbContact.ContactId;
            //if (dbContact.Confirmed == false)
            //    return dialogueInfo;

            //// если он подтвержден
            //// ищим диалог с этим контактом
            //// если диалога не существует, то создаем его
            //if (dbUserDialogue == null)
            //{
            //    var currentDate = DateTime.Today;
            //    var dbDialogue = new DbDialogue()
            //    {

            //        CreateDate = currentDate,
            //        IsMultyUser = false,
            //        RecentActivityDate = currentDate,
            //    };
            //    _dialogueRepository.Insert(dbDialogue);

            //    dbUserDialogue = new DbUserDialogue()
            //    {
            //        DialogueId = dbDialogue.Id,
            //        UserId = dbContact.OwnerId,
            //        HashCode = hashcode,
            //        DialogueName = "qweqwe"
            //    };
            //    _userDialogueRepository.Insert(dbUserDialogue);
            //}
            // если диалог существует, то берем его

            return dialogueInfo;
        }

        public int CreateDialogue(string firstUserId, string secondUserId)
        {
            var currentDate = DateTime.Now;
            var dbDialogue = new DbDialogue()
            {
                CreateDate = currentDate,
                IsMultyUser = false,
                RecentActivityDate = currentDate,
            };
            
            int createdDialogueId = _dialogueRepository.Insert(dbDialogue);
            return createdDialogueId;
        }

        public int? GetDialogueIdByHashCode(int hashCode)
        {
            var userDialogue = _userDialogueRepository.SearchFor(x=>x.HashCode == hashCode).SingleOrDefault();
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

        public int GetDialogueHashCode(string userId1, string userId2)
        {
            var result = userId1.GetHashCode() + userId2.GetHashCode();
            return result;
        }


    }
}