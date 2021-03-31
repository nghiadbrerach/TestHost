using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebEnterprise.Models;
namespace WebEnterprise.Controllers
{
    public class ContentAssignActionController : Controller
    {
        private G5EnterpriseDBEntities db = new G5EnterpriseDBEntities();
        // GET: ContentAssignAction
        public ActionResult View()
        {
            return View();
        }
    }
}