using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebChat.Classes;

namespace WebChat.Controllers
{
    public class ImController : Controller
    {
        private Repository<DbDialogue> _dialoguesRepository = null;

        public async Task<ActionResult> Index()
        {
            return View();
        }

        public async Task<ActionResult> Index(string dialogueId)
        {
            return PartialView();
        }
    }
}