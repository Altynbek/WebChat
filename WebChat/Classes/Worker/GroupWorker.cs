using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using WebChat.Classes.Db.Structure;
using WebChat.Classes.DB.Repositories;
using WebChat.Classes.Exceptions;
using WebChat.Models.Home;
using WebChat.Models.Im;

namespace WebChat.Classes.Worker
{
    public class GroupWorker : IDisposable
    {
        //private readonly GroupDialogueRepository _groupRepository = null;

        private readonly UserDialogueRepository _userDialogueRepository = null;
        private readonly DialogueRepository _dialogueRepository = null;

        public GroupWorker(DbContext context)
        {
            _dialogueRepository = new DialogueRepository(context);
            _userDialogueRepository = new UserDialogueRepository(context);
        }


        public GroupListModel GetGroups(string userId)
        {
            if (String.IsNullOrEmpty(userId))
                throw new ArgumentNullException("userId");

            var groupList = new GroupListModel();

            var dbUserDialogues = _userDialogueRepository.SearchFor(x => x.UserId == userId).ToList();
            var userDialoguesId = dbUserDialogues.Select(x => x.DialogueId).ToList();
            var dbUserGroupDialogues = _dialogueRepository.SearchFor(x => x.IsMultyUser == true && userDialoguesId.Contains(x.Id)).ToList();

            foreach(var groupDialogue in dbUserGroupDialogues)
            {
                var companionsId = _userDialogueRepository.SearchFor(x => x.DialogueId == groupDialogue.Id).Select(x=>x.UserId).ToList();
                GroupModel m = new GroupModel()
                {
                    DialogueId = groupDialogue.Id,
                    Name = groupDialogue.Name,
                    PhotoUrl = groupDialogue.PhotoUrl,
                    RecentActivityDate = groupDialogue.RecentActivityDate,
                    CompanionsId = companionsId
                };

                groupList.Groups.Add(m);
            }
            return groupList;
        }

        public int CreateGroupDialogue(string userCreatorId, List<string> companionsId, string dialogueName, string groupImage)
        {
            if (String.IsNullOrEmpty(userCreatorId))
                throw new ArgumentNullException("userCreatorId");

            companionsId.Add(userCreatorId);

            var currentDate = DateTime.Now;
            var dbDialogue = new DbDialogue()
            {
                CreateDate = currentDate,
                IsMultyUser = true,
                Name = dialogueName,
                PhotoUrl = groupImage,
                RecentActivityDate = currentDate
            };
            int dialogueId = -1;
            _dialogueRepository.Insert(dbDialogue, out dialogueId);

            int hash = GetGroupDialogueHashCode(companionsId);
            var dbUserDialogues = new List<DbUserDialogue>();
            foreach (string userId in companionsId)
            {
                var dbUserDialogue = new DbUserDialogue()
                {
                    DialogueId = (int)dialogueId,
                    UserId = userId,
                    DialogueName = null,
                    HashCode = hash
                };
                dbUserDialogues.Add(dbUserDialogue);
            }
            _dialogueRepository.Insert(dbUserDialogues);
            return (int)dialogueId;
        }

        public int? GetDialogueIdByHashCode(int hashCode)
        {
            var userDialogue = _userDialogueRepository.SearchFor(x => x.HashCode == hashCode).FirstOrDefault();
            if (userDialogue != null)
            {
                return userDialogue.DialogueId;
            }
            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetGroupDialogueHashCode(List<string> companionsId)
        {
            int hash = 0;
            foreach (var item in companionsId)
            {
                hash += item.GetHashCode();
            }

            return hash;
        }

        public GroupDialogueInfoModel GetGroupDialogueInfo(int dialogueId)
        {
            var dbDialogue = _dialogueRepository.GetById(dialogueId);

            var dialogueInfo = new GroupDialogueInfoModel()
            {
                DialogueId = dialogueId,
                Name = dbDialogue.Name,
                PhotoUrl = dbDialogue.PhotoUrl,
                RecentActivityDate = dbDialogue.RecentActivityDate,
            };
            dialogueInfo.Companions = _dialogueRepository.GetDialogueCompanions(dialogueId);
            return dialogueInfo;
        }


        public GroupModel GetById(int dialogueId)
        {
            var dbDialogue = _dialogueRepository.GetById(dialogueId);

            var group = new GroupModel()
            {
                DialogueId = dbDialogue.Id,
                HasNewMessages = false,
                Name = dbDialogue.Name,
                RecentActivityDate = dbDialogue.RecentActivityDate,
                PhotoUrl = dbDialogue.PhotoUrl
            };
            return group;
        }

        public void Dispose()
        {
            _userDialogueRepository.Dispose();
            _dialogueRepository.Dispose();
        }
    }
}