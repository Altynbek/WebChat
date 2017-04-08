using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebChat.Classes.Db.Structure;
using WebChat.Classes.DB.Repositories;
using WebChat.Classes.Worker;
using WebChat.Models;
using WebChat.Models.Account;

namespace WebChat.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private ContactWorker _contactWorker;
        private UserRepository _userRepository;

        public AccountController()
        {
            var context = new DbContext();
            _contactWorker = new ContactWorker(context);
            _userRepository = new UserRepository(context);
        }

        [HttpGet]
        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Signup(SignUpModel model)
        {
            if (ModelState.IsValid)
            {
                string photoUrl = "/Content/Images/" + model.UserPhoto == null ? "avatar-default.png" : model.UserPhoto.FileName;
                var user = new DbUser() { Email = model.Email, UserName = model.Name, PhotoUrl = photoUrl };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if (model.UserPhoto != null && model.UserPhoto.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(model.UserPhoto.FileName);
                        var path = Path.Combine(Server.MapPath("/Content/Images"), fileName);
                        model.UserPhoto.SaveAs(path);
                    }
                    return RedirectToAction("SignIn", "Account");
                }

                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult SignIn(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SignIn(SignInModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.Email, model.Password);
                if (user == null)
                    ModelState.AddModelError("", "Incorrect password or email");
                else
                {
                    ClaimsIdentity claim = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                    var signInProperties = new AuthenticationProperties() { IsPersistent = true };
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(signInProperties, claim);

                    if (String.IsNullOrEmpty(returnUrl))
                        return RedirectToAction("Index", "Home");
                    return Redirect(returnUrl);
                }
            }

            ViewBag.returnUrl = returnUrl;
            return View(model);
        }

        public ActionResult SignOut()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("SignIn");
        }


        public ActionResult FindUsers(string searchedString)
        {
            string currentUserId = User.Identity.GetUserId();
            var existedContactsId = _contactWorker.GetContacts(currentUserId).Contacts.Select(x => x.ContactId);
            var dbUsers = _userRepository.SearchFor(e => e.UserName.Contains(searchedString) && !existedContactsId.Contains(e.Id)).ToList();

            var model = new SearchedContactListModel();
            foreach (var dbUser in dbUsers)
            {
                model.Contacts.Add(new SearchedContactModel() { Id = dbUser.Id, Name = dbUser.UserName, PhotoUrl = dbUser.PhotoUrl });
            }
            return PartialView(model);
        }

        protected override void Dispose(bool disposing)
        {
            _contactWorker.Dispose();
            _userRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}