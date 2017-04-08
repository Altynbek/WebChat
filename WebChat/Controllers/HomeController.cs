using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using WebChat.Classes.Db.Structure;
using WebChat.Classes.Worker;
using WebChat.Models.Contact;
using WebChat.Models.Home;

namespace WebChat.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ContactWorker _contactWorker = null;
        private GroupWorker _groupWorker = null;

        public HomeController()
        {
            var context = new DbContext();
            _contactWorker = new ContactWorker(context);
            _groupWorker = new GroupWorker(context);
        }

        public ActionResult Index()
        {
            ContactListModel model = _contactWorker.GetContacts(User.Identity.GetUserId());
            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            _contactWorker.Dispose();
            _groupWorker.Dispose();
            base.Dispose(disposing);
        }
    }
}