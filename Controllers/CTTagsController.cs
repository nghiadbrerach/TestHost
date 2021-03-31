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
    public class CTTagsController : Controller
    {
        private G5EnterpriseDBEntities3 db = new G5EnterpriseDBEntities3();

        // GET: CTTags
        public ActionResult Index()
        {
            return View(db.CTTags.ToList());
        }

        // GET: CTTags/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTTag cTTag = db.CTTags.Find(id);
            if (cTTag == null)
            {
                return HttpNotFound();
            }
            return View(cTTag);
        }

        // GET: CTTags/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CTTags/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CTTagID,CTTag1,CTTagDescription")] CTTag cTTag)
        {
            if (ModelState.IsValid)
            {
                db.CTTags.Add(cTTag);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cTTag);
        }

        // GET: CTTags/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTTag cTTag = db.CTTags.Find(id);
            if (cTTag == null)
            {
                return HttpNotFound();
            }
            return View(cTTag);
        }

        // POST: CTTags/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CTTagID,CTTag1,CTTagDescription")] CTTag cTTag)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cTTag).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cTTag);
        }

        // GET: CTTags/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTTag cTTag = db.CTTags.Find(id);
            if (cTTag == null)
            {
                return HttpNotFound();
            }
            return View(cTTag);
        }

        // POST: CTTags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CTTag cTTag = db.CTTags.Find(id);
            db.CTTags.Remove(cTTag);
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
    }
}
