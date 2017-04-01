using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.Mvc;
using WebChat.Classes.Db.Structure;
using WebChat.Classes.Worker;
using WebChat.Models.Im;

namespace WebChat.Controllers
{
    [Authorize]
    public class ImController : Controller
    {
        private readonly DialogueWorker _dialogueWorker = null;
        private readonly MessageWorker _messageWorker = null;
        private readonly ContactWorker _contactWorker = null;


        public ImController()
        {
            var context = new DbContext();
            _dialogueWorker = new DialogueWorker(context);
            _messageWorker = new MessageWorker(context);
            _contactWorker = new ContactWorker(context);
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

        [HttpPost]
        public ActionResult SendMessage(string dialogueId, string companionId, string message)
        {
            string currentUserId = User.Identity.GetUserId();
            int hashCode = DialogueWorker.GetDialogueHashCode(currentUserId, companionId);
            int? dbDialogueId = _dialogueWorker.GetDialogueIdByHashCode(hashCode);

            if(dbDialogueId == null)
            {
                dbDialogueId = _dialogueWorker.CreatePersonalDialogue(currentUserId, companionId);
            }

            var currentDate = DateTime.Now;
            var dbMessage = new DbMessage()
            {
                CreatorId = currentUserId,
                DialogueId = (int)dbDialogueId,
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
            messageModel.dialogueId = (int)dbDialogueId;

            return PartialView("Message", messageModel);
        }

        [HttpPost]
        public JsonResult MarkDialogueAsReaded(string dialogueId)
        {
            var currentUserId = User.Identity.GetUserId();
            int updateRecordsCount = _messageWorker.MarkMessagesAsReaded(int.Parse(dialogueId), currentUserId);
            return Json(new { success = updateRecordsCount > 0}, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            _dialogueWorker.Dispose();
            _messageWorker.Dispose();
            _contactWorker.Dispose();
            base.Dispose(disposing);
        }
    }
}