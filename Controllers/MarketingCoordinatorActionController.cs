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
    public class MarketingCoordinatorActionController : Controller
    {
        // GET: MarketingCoordinatorAction
        private G5EnterpriseDBEntities db = new G5EnterpriseDBEntities();
        public ActionResult MCProfile()
        {
            var UserName = User.Identity.Name;
            var userId = UserName;

            var MC = (from a in db.MarketingCoordinators where a.UserName.ToString().Equals(userId) select a).FirstOrDefault();
            return View(MC);
        }

        [Authorize(Roles = "Admin,MarketingCoordinator")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MarketingCoordinator marketingCoordinator = db.MarketingCoordinators.Find(id);
            if (marketingCoordinator == null)
            {
                return HttpNotFound();
            }
            ViewBag.MCID = (from i in db.MarketingCoordinators where i.UserName == User.Identity.Name select i.MCID).FirstOrDefault();
            ViewBag.uName = (from m in db.MarketingCoordinators where m.UserName == User.Identity.Name select m.UserName).FirstOrDefault();
            ViewBag.FacName = (from c in db.MarketingCoordinators where c.UserName == User.Identity.Name select c.Faculty.FacultyName).FirstOrDefault();
            //ViewBag.FacultyID = new SelectList(db.Faculties, "FacultyID", "FacultyName", marketingCoordinator.FacultyID);
            return View(marketingCoordinator);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit([Bind(Include = "MCEmail,MCID,MCName,MCAddress,MCPhone,FacultyID,UserName")] MarketingCoordinator marketingCoordinator)
        {
            var mID = (from i in db.MarketingCoordinators where i.UserName == User.Identity.Name select i.MCID).FirstOrDefault();
            var uName = (from m in db.MarketingCoordinators where m.UserName == User.Identity.Name select m.UserName).FirstOrDefault();
            var facID = (from c in db.MarketingCoordinators where c.UserName == User.Identity.Name select c.FacultyID).FirstOrDefault();
            if (ModelState.IsValid)
            {
                marketingCoordinator.MCID = mID;
                marketingCoordinator.FacultyID = facID;
                marketingCoordinator.UserName = uName;
                db.Entry(marketingCoordinator).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("MCProfile");
            }

            //ViewBag.FacultyID = new SelectList(db.Faculties, "FacultyID", "FacultyName", marketingCoordinator.FacultyID);
            return View(marketingCoordinator);
        }
    }
}