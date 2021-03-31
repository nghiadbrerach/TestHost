using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebEnterprise.Models;

namespace WebEnterprise.Controllers
{
    public class MarketingCoordinatorsController : Controller
    {
        private G5EnterpriseDBEntities db = new G5EnterpriseDBEntities();
        [Authorize(Roles = "Admin,MarketingCoordinator")]
        // GET: MarketingCoordinators
        public ActionResult Index()
        {
            var marketingCoordinators = db.MarketingCoordinators.Include(m => m.Faculty).Include(m => m.Faculty);
            return View(marketingCoordinators.ToList());
        }

        // GET: MarketingCoordinators/Details/5
        [Authorize(Roles = "Admin,MarketingCoordinator")]
        public ActionResult Details(string id)
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
            return View(marketingCoordinator);
        }

        // GET: MarketingCoordinators/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {

            ViewBag.FacultyID = new SelectList(db.Faculties, "FacultyID", "FacultyName");
            return View();
        }

        // POST: MarketingCoordinators/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create([Bind(Include = "MCEmail,MCID,MCName,MCAddress,MCPhone,FacultyID,UserName")] MarketingCoordinator marketingCoordinator)
        {
            if (ModelState.IsValid)
            {
                db.MarketingCoordinators.Add(marketingCoordinator);
                db.SaveChanges();

                AuthenController.CreateAccount(marketingCoordinator.UserName, "123456", "MarketingCoordinator");

                return RedirectToAction("Index");
            }

            ViewBag.CTTagID = new SelectList(db.Faculties, "FacultyID", "FacultyName", marketingCoordinator.FacultyID);
            return View(marketingCoordinator);
        }

        // GET: MarketingCoordinators/Edit/5
        [Authorize(Roles = "Admin,MarketingCoordinator")]
        public ActionResult Edit(string id)
        {
            var mID = (from i in db.MarketingCoordinators where i.UserName == User.Identity.Name select i.MCID).FirstOrDefault();
            var uName = (from m in db.MarketingCoordinators where m.UserName == User.Identity.Name select m.UserName).FirstOrDefault();
            var facID = (from c in db.MarketingCoordinators where c.UserName == User.Identity.Name select c.FacultyID).FirstOrDefault();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MarketingCoordinator marketingCoordinator = db.MarketingCoordinators.Find(id);
            if (marketingCoordinator == null)
            {
                return HttpNotFound();
            }

            ViewBag.FacultyID = new SelectList(db.Faculties, "FacultyID", "FacultyName", marketingCoordinator.FacultyID);
            return View(marketingCoordinator);
        }

        // POST: MarketingCoordinators/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit([Bind(Include = "MCEmail,MCID,MCName,MCAddress,MCPhone,FacultyID,UserName")] MarketingCoordinator marketingCoordinator)
        {
            var mID = (from i in db.MarketingCoordinators where i.UserName == User.Identity.Name select i.MCID).FirstOrDefault();
            var uName = (from m in db.MarketingCoordinators where m.UserName == User.Identity.Name select m.UserName).FirstOrDefault();
            var facID = (from c in db.MarketingCoordinators where c.UserName == User.Identity.Name select c.FacultyID).FirstOrDefault();
            if (ModelState.IsValid)
            {
                db.Entry(marketingCoordinator).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FacultyID = new SelectList(db.Faculties, "FacultyID", "FacultyName", marketingCoordinator.FacultyID);
            return View(marketingCoordinator);
        }

        // GET: MarketingCoordinators/Delete/5
        [Authorize(Roles = "Admin,MarketingCoordinator")]
        public ActionResult Delete(string id)
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
            return View(marketingCoordinator);
        }

        // POST: MarketingCoordinators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeleteConfirmed(string id)
        {
            MarketingCoordinator marketingCoordinator = db.MarketingCoordinators.Find(id);
            db.MarketingCoordinators.Remove(marketingCoordinator);
            db.SaveChanges();
            AuthenController.DeleteAccount(marketingCoordinator.UserName);
            return RedirectToAction("Index");
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}