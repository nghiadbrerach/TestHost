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
    public class StudentActionController : Controller
    {
        private G5EnterpriseDBEntities db = new G5EnterpriseDBEntities();
        // GET: StudentAction
        public ActionResult StdProfile()
        {
            var UserName = User.Identity.Name;
            var userId = UserName;

            var student = (from b in db.Students where b.UserName.ToString().Equals(userId) select b).FirstOrDefault();
            return View(student);
        }
        [Authorize(Roles = "Student,Admin")]
        public ActionResult Edit(string id)
        {
            ViewBag.StdID = (from i in db.Students where i.UserName == User.Identity.Name select i.StudentID).FirstOrDefault();
            ViewBag.Fac = (from i in db.Students where i.UserName == User.Identity.Name select i.Faculty.FacultyName).FirstOrDefault();

            var Name = (from m in db.Students where m.UserName == User.Identity.Name select m).FirstOrDefault();
            ViewBag.uName = Name.UserName;
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

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit([Bind(Include = "StudentEmail,StudentName,StudentID,StudentAddress,FacultyID,DOB,UserName")] Student student)
        {
            var stdID = (from i in db.Students where i.UserName == User.Identity.Name select i.StudentID).FirstOrDefault();
            var uName = (from m in db.Students where m.UserName == User.Identity.Name select m.UserName).FirstOrDefault();
            var facID = (from c in db.Students where c.UserName == User.Identity.Name select c.FacultyID).FirstOrDefault();
            if (ModelState.IsValid)
            {
                student.StudentID = stdID;
                student.FacultyID = facID;
                student.UserName = uName;
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("StdProfile");
            }

            return View(student);
        }

    }
}