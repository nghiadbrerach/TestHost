using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebEnterprise.Models;

namespace WebEnterprise.Controllers
{
    public class HomeController : Controller
    {
        private G5EnterpriseDBEntities db = new G5EnterpriseDBEntities();
        public ActionResult Index(string searchString)
        {
            var tp = from m in db.Topics select m;
            if (!String.IsNullOrEmpty(searchString))
            {
                tp = tp.Where(s => s.TopicID.Contains(searchString));
            }
            return View(tp);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact page.";

            return View();
        }
    }
}