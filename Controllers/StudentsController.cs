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
    public class StudentsController : Controller
    {
        private G5EnterpriseDBEntities db = new G5EnterpriseDBEntities();
        [Authorize(Roles = "Admin")]
        // GET: Students
        public ActionResult Index()
        {
            return View(db.Students.ToList());
        }

        // GET: Students/Details/5
        [Authorize(Roles = "Admin,Student")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.FacultyID = new SelectList(db.Faculties, "FacultyID", "FacultyName");
            return View(student);
        }
        [Authorize(Roles = "Admin")]
        // GET: Students/Create
        public ActionResult Create()
        {
            ViewBag.FacultyID = new SelectList(db.Faculties, "FacultyID", "FacultyName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Create([Bind(Include = "StudentEmail,StudentID,StudentName,StudentAddress,DOB,UserName,FacultyID")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();

                AuthenController.CreateAccount(student.UserName, "123456", "Student");

                return RedirectToAction("Index");
            }

            ViewBag.FacultyID = new SelectList(db.Faculties, "FacultyID", "FacultyName", student.FacultyID);

            return View(student);
        }

        // GET: Students/Edit/5
        [Authorize(Roles = "Student,Admin")]
        public ActionResult Edit(string id)
        {
            ViewBag.StdID = (from i in db.Students where i.UserName == User.Identity.Name select i.StudentID).FirstOrDefault();
            ViewBag.Fac = (from i in db.Students where i.UserName == User.Identity.Name select i.Faculty.FacultyName).FirstOrDefault();
            ViewBag.UName = (from s in db.Students
                             where s.UserName == User.Identity.Name
                             select s.UserName).FirstOrDefault();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.FacultyID = new SelectList(db.Faculties, "FacultyID", "FacultyName");
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public ActionResult Edit([Bind(Include = "StudentEmail,StudentID,StudentName,StudentAddress,DOB,UserName,FacultyID")] Student student)
        {
            ViewBag.StdID = (from i in db.Students where i.UserName == User.Identity.Name select i.StudentID).FirstOrDefault();
            ViewBag.Fac = (from i in db.Students where i.UserName == User.Identity.Name select i.Faculty.FacultyName).FirstOrDefault();
            ViewBag.FacultyID = new SelectList(db.Faculties, "FacultyID", "FacultyName", student.FacultyID);
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(student);
        }

        // GET: Students/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        
        public ActionResult DeleteConfirmed(string id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            AuthenController.DeleteAccount(student.UserName);
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
