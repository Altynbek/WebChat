using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebChat.Classes.Db.Structure;
using WebChat.Classes.Helpers;
using WebChat.Classes.Worker;
using WebChat.Models.Contact;
using WebChat.Models.Im;

namespace WebChat.Controllers
{
    [Authorize]
    public class ImController : Controller
    {
        private readonly DialogueWorker _dialogueWorker = null;
        private readonly MessageWorker _messageWorker = null;
        private readonly ContactWorker _contactWorker = null;
        private readonly GroupWorker _groupWorker = null;


        public ImController()
        {
            var context = new DbContext();
            _dialogueWorker = new DialogueWorker(context);
            _messageWorker = new MessageWorker(context);
            _contactWorker = new ContactWorker(context);
            _groupWorker = new GroupWorker(context);
        }

        public ActionResult Index(string contactId)
        {
            int id = int.Parse(contactId);
            var dialogueInfo = _dialogueWorker.GetDialogueInfo(id);

            var companionContact = _contactWorker.GetContactById(id);
            string currentUserId = User.Identity.GetUserId();
            int hashCode = DialogueWorker.GetDialogueHashCode(currentUserId, companionContact.ContactId);
            int? dbDialogueId = _dialogueWorker.GetDialogueIdByHashCode(hashCode);

            if (dbDialogueId != null)
            {
                dialogueInfo.Messages = _messageWorker.GetMessages((int)dbDialogueId).OrderBy(x => x.SendTime).ToList();
                dialogueInfo.DialogueId = (int)dbDialogueId;
            }

            return PartialView("Dialogue", dialogueInfo);
        }

        public ActionResult StartGroupDialogue(string dialogueId)
        {
            int id = int.Parse(dialogueId);
            var dialogueInfo = _groupWorker.GetGroupDialogueInfo(id);

            dialogueInfo.Messages = _messageWorker.GetMessages(int.Parse(dialogueId)).OrderBy(x => x.SendTime).ToList();
            return PartialView("GroupDialogue", dialogueInfo);
        }

        [HttpPost]
        public ActionResult SendMessage(string dialogueId, string companionId, string message)
        {
            string currentUserId = User.Identity.GetUserId();
            int dialId = -1;
            if(!String.IsNullOrEmpty(companionId))
            {
                int hashCode = DialogueWorker.GetDialogueHashCode(currentUserId, companionId);
                int? dbDialogueId = _dialogueWorker.GetDialogueIdByHashCode(hashCode);

                dialId = dbDialogueId == null 
                    ? _dialogueWorker.CreatePersonalDialogue(currentUserId, companionId) 
                    : (int)dbDialogueId;
                
            }
            else if (!String.IsNullOrEmpty(dialogueId))
            {
                dialId = int.Parse(dialogueId);
            }


                var currentDate = DateTime.Now;
            var dbMessage = new DbMessage()
            {
                CreatorId = currentUserId,
                DialogueId = dialId,
                IsEdited = false,
                SendingDate = currentDate,
                Text = message,
                IsReaded = false
            };
            _messageWorker.SaveMessage(dbMessage);

            var messageModel = new MessageModel();
            messageModel.message = message;
            messageModel.SendTime = DateTime.Now;
            messageModel.SenderName = User.Identity.Name;
            messageModel.dialogueId = dialId;

            return PartialView("Message", messageModel);
        }

        [HttpPost]
        public JsonResult MarkDialogueAsReaded(string dialogueId)
        {
            var currentUserId = User.Identity.GetUserId();
            int updateRecordsCount = _messageWorker.MarkMessagesAsReaded(int.Parse(dialogueId), currentUserId);
            return Json(new { success = updateRecordsCount > 0 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateGroup(ModalContactListModel model)
        {
            int dialogueId = -1;
            string viewMarkup = "";
            if (model != null)
            {
                string currentUserId = User.Identity.GetUserId();
                string groupPhotoUrl = "";
                var companionsId = model.Contacts.Where(x => x.Selected).Select(x => x.CompanionId).ToList();
                dialogueId = _groupWorker.CreateGroupDialogue(currentUserId, companionsId, model.GroupName, groupPhotoUrl);

                var dialogueModel = _groupWorker.GetById(dialogueId);
                viewMarkup = RazorHelper.RenderRazorViewToString(this, "GroupListItem", dialogueModel);
                return Json(new { success = true, createdDialogueId = dialogueId, htmlMarkup = viewMarkup });
            }
            return Json(new { success = false });
        }

        public ActionResult GetGroups()
        {
            string currentUserId = User.Identity.GetUserId();
            var groups = _groupWorker.GetGroups(currentUserId);
            return PartialView("GroupList", groups);
        }

        protected override void Dispose(bool disposing)
        {
            _dialogueWorker.Dispose();
            _messageWorker.Dispose();
            _contactWorker.Dispose();
            _groupWorker.Dispose();

            base.Dispose(disposing);
        }
    }
}