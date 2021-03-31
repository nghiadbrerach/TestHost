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
    public class TopicController : Controller
    {
        private G5EnterpriseDBEntities db = new G5EnterpriseDBEntities();
        [Authorize(Roles = "Admin,Guest")]// GET: Topic
        public ActionResult Index(string searchString)
        {
            var tp = from m in db.Topics select m;
            if (!String.IsNullOrEmpty(searchString))
            {
                tp = tp.Where(s => s.TopicID.Contains(searchString));
            }
            return View(tp);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.FacultyID = new SelectList(db.Faculties, "FacultyID", "FacultyName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Topic tp)
        {
            ViewBag.FacultyID = new SelectList(db.Faculties, "FacultyID", "FacultyName", tp.FacultyID);
            if (tp.EndDate > tp.StartDate)
            {


                if (ModelState.IsValid)
                {

                    try
                    {
                        db.Topics.Add(tp);
                        db.SaveChanges();
                        return RedirectToAction("index");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex);
                        ModelState.AddModelError("", "Error inserting Content. ID is already existed");
                        return View(tp);
                    }
                }

            }
            else
            {
                TempData["message"] = "The End Date must come after the Start Date";
                return View();
            }
            return RedirectToAction("index");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Details(string id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic tp = db.Topics.Find(id);
            if (tp == null)
            {
                return HttpNotFound();
            }
            return View(tp);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic tp = db.Topics.Find(id);
            if (tp == null)
            {
                return HttpNotFound();
            }
            return View(tp);
        }
        [HttpPost, ActionName("Delete")]

        public ActionResult DeleteConfirmed(string id)
        {
            Topic tp = db.Topics.Find(id);
            db.Topics.Remove(tp);
            db.SaveChanges();
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
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            ViewBag.FacultyID = new SelectList(db.Faculties, "FacultyID", "FacultyName");
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic tp = db.Topics.Find(id);
            ViewBag.TopicID = (from i in db.Topics 
                             where i.TopicID == id 
                             select i.TopicID).FirstOrDefault();
            if (tp == null)
            {
                return HttpNotFound();
            }

            return View(tp);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Topic tp,string id)
        {

            ViewBag.TopicID = (from i in db.Topics
                               where i.TopicID == id
                               select i.TopicID).FirstOrDefault();
            ViewBag.FacultyID = new SelectList(db.Faculties, "FacultyID", "FacultyName", tp.FacultyID);
            if (tp.EndDate > tp.StartDate)
            {

                if (ModelState.IsValid)
                {
                    tp.TopicID = ViewBag.TopicID;
                    db.Entry(tp).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["message"] = "The End Date must come after the Start Date";
                return View();
            }
            return View(tp);
        }
    }
}
