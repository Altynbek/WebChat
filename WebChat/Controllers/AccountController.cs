using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebChat.Classes.Db.Structure;
using WebChat.Classes.DB.Repositories;
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
                var user = new DbUser() { Email = model.Email, UserName = model.Name };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                    return RedirectToAction("SignIn", "Account");
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
            using (var context = new DbContext())
            {
                var repository = new UserRepository(context);
                var dbUsers = repository.SearchFor(e => e.UserName.Contains(searchedString)).ToList();
                var model = new SearchedContactListModel();
                foreach (var dbUser in dbUsers)
                {
                    model.Contacts.Add(new SearchedContactModel() { Id = dbUser.Id, Name = dbUser.UserName, PhotoUrl = dbUser.PhotoUrl });
                }
                return PartialView(model);
            }
        }
    }
}