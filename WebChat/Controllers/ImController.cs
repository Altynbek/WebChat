using Microsoft.AspNet.Identity;
using System;
using System.Web.Mvc;
using WebChat.Classes.Worker;
using WebChat.Models.Im;

namespace WebChat.Controllers
{
    [Authorize]
    public class ImController : Controller
    {
        private readonly DialogueWorker _dialogueWorker = null;
        public ImController()
        {
            _dialogueWorker = new DialogueWorker(new Classes.Db.Structure.DbContext());
        }

        public ActionResult Index(string contactId)
        {
            var dialogueInfo = _dialogueWorker.GetDialogueInfo(int.Parse(contactId));

            return PartialView("Dialogue", dialogueInfo);
        }

        [HttpPost]
        public ActionResult SendMessage(string dialogueId, string companionId, string message)
        {
            var messageModel = new MessageModel();
            messageModel.message = message;
            messageModel.SendTime = DateTime.Now;
            messageModel.SenderName = User.Identity.Name;
            //messageModel.dialogueId = int.Parse(dialogueId);


            // получить идентификатор диалога с этим контактом на основе companionId
            string currentUserId = User.Identity.GetUserId();
            int hashCode = _dialogueWorker.GetDialogueHashCode(currentUserId, companionId);
            int? dbDialogueId = _dialogueWorker.GetDialogueIdByHashCode(hashCode);

            if(dbDialogueId == null)
            {
                // если такого диалога нет, то создаем его
            }

            // после того как создали диалог, мы заполняем поля объекта DbDialogueMessage
            // сохранияем  в базу


            return PartialView("Message", messageModel);
        }


        protected override void Dispose(bool disposing)
        {
            _dialogueWorker.Dispose();
            base.Dispose(disposing);
        }
    }
}