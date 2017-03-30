using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using WebChat.Classes.Db.Structure;
using WebChat.Classes.Worker;
using WebChat.Models.Contact;

namespace WebChat.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var context = new DbContext())
            {
                ContactWorker contactWorker = new ContactWorker(context);
                ContactListModel model = contactWorker.GetContacts(User.Identity.GetUserId());
                return View(model);
            }
        }
    }
}