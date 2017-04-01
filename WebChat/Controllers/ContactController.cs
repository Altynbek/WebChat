using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebChat.Classes.Db.Structure;
using WebChat.Classes.DB.Repositories;
using WebChat.Classes.Worker;
using WebChat.Models;
using WebChat.Models.Contact;

namespace WebChat.Controllers
{
    [Authorize]
    public class ContactController : Controller
    {
        private readonly ContactRepository _contactRepository = null;
        private readonly UserRepository _userRepository = null;

        public ContactController()
        {
            var context = new DbContext();
            _contactRepository = new ContactRepository(context);
            _userRepository = new UserRepository(context);
        }

        [HttpGet]
        public ActionResult AddNewContact(string contactId)
        {
            var usr = _userRepository.GetById(contactId);
            var contact = _contactRepository.GetByUserId(contactId);

            var model = new NewContactModel() { ContactId = contactId, ContactName = usr.UserName, PhotoUrl = usr.PhotoUrl };
            return PartialView("AddNewContactToBook", model);
        }

        [HttpPost]
        public JsonResult AddNewContact(NewContactModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUserId = User.Identity.GetUserId();
                var contact = new DbUserContact()
                {
                    Confirmed = false,
                    ContactId = model.ContactId,
                    OwnerId = currentUserId,
                    FriendsheepInitiator = currentUserId
                };
                var contact2 = new DbUserContact()
                {
                    Confirmed = false,
                    ContactId = currentUserId,
                    OwnerId = model.ContactId,
                    FriendsheepInitiator = currentUserId
                };

                _contactRepository.Insert(contact);
                _contactRepository.Insert(contact2);
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult DeleteContact(string companionId)
        {
            int result = _contactRepository.DeleteById(User.Identity.GetUserId(), companionId);
            return Json(new { success = result > 0 });
        }

        [HttpPost]
        public JsonResult AcceptContact(string companionId)
        {
            int result = _contactRepository.AcceptContact(User.Identity.GetUserId(), companionId);
            return Json(new { success = result > 0 });
        }

        protected override void Dispose(bool disposing)
        {
            _contactRepository.Dispose();
            _userRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}