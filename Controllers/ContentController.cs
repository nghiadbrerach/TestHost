using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data;
using WebEnterprise.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.IO;
using System.Net.Mail;

namespace WebEnterprise.Controllers
{
    //[Authorize(Roles = "Student,Guest")]
    public class ContentController : Controller
    {
        private G5EnterpriseDBEntities db = new G5EnterpriseDBEntities();
        // GET: Content
        [Authorize(Roles = "Student,MarketingCoordinator")]

        public ActionResult Topic()
        {
            var tp = from m in db.Topics select m;
            //var tp = db.Topics
            //    .Where(s => s.C)
            //    .ToList();
            return View(tp);
        }

        public ActionResult Uploaded()
        {
            var ct = db.Contents
                .Where(s => s.Student.UserName == User.Identity.Name)
                .ToList();

            return View(ct);
        }
        public ActionResult Search(string searchString)
        {
            var ctsearch = from m in db.Contents select m;
            if (!String.IsNullOrEmpty(searchString))
            {
                ctsearch = ctsearch.Where(s => s.CTName.Contains(searchString) && s.Student.UserName == User.Identity.Name && s.FacultyID == s.Student.FacultyID);
            }
            else
            {
                return RedirectToAction("Uploaded");
            }
            return View(ctsearch);
        }
        [Authorize(Roles = "Student")]
        public ActionResult Create(string id)
        {
            var student = (from s in db.Students
                           where s.UserName.Equals(User.Identity.Name)
                           select s).FirstOrDefault();
            var tpid = (from t in db.Topics
                        where t.TopicID == id
                        select t).FirstOrDefault();
            ViewBag.StudentName = student.StudentName; //LINQ+ ENtityframework
            ViewBag.StudentID = student.StudentID;
            ViewBag.Faculty = student.Faculty.FacultyName;
            ViewBag.TName = tpid.TopicName;

            return View();


        }
        [HttpPost]
        public ActionResult Create(Content ct, HttpPostedFileBase postedImg, HttpPostedFileBase postedPDF, string id, ContentAssign contentAssign)
        {
            var student = (from s in db.Students
                           where s.UserName.Equals(User.Identity.Name)
                           select s).FirstOrDefault();
            ct.StudentID = student.StudentID; //LINQ+ ENtityframework


            var tpid = (from t in db.Topics
                        where t.TopicID == id
                        select t).FirstOrDefault();
            ct.FacultyID = tpid.FacultyID;

            byte[] bytes;
            byte[] byte2s;
            using (BinaryReader br = new BinaryReader(postedImg.InputStream))
            {
                bytes = br.ReadBytes(postedImg.ContentLength);
            }
            using (BinaryReader br2 = new BinaryReader(postedPDF.InputStream))
            {
                byte2s = br2.ReadBytes(postedPDF.ContentLength);
            }

            if (DateTime.Now < tpid.EndDate)
            {
                db.Contents.Add(new Content
                {
                    Name2 = Path.GetFileName(postedImg.FileName),
                    ContentType2 = postedImg.ContentType,
                    Data2 = bytes,
                    CTName = ct.CTName,
                    CTDescription = ct.CTDescription,
                    FacultyID = ct.FacultyID,
                    StudentID = ct.StudentID,
                    Name = Path.GetFileName(postedPDF.FileName),
                    ContentType = postedPDF.ContentType,
                    Data = byte2s,
                    TopicID = id
                });

                var mcmail = (from m in db.MarketingCoordinators where m.FacultyID == ct.FacultyID select m.MCEmail).FirstOrDefault();
                var mcid = (from m in db.MarketingCoordinators where m.FacultyID == ct.FacultyID select m.MCID).FirstOrDefault();
                using (MailMessage mm = new MailMessage("sysgwww@gmail.com", mcmail))
                {
                    //ct.To = ViewBag.McMail;
                    mm.Subject = "Grade content";
                    mm.Body = "You have a new content from student: " + student.StudentName + "--- Stduent ID: " + student.StudentID;
                    mm.IsBodyHtml = false;
                    using (SmtpClient smtp = new SmtpClient())
                    {
                        smtp.Host = "smtp.gmail.com";
                        smtp.EnableSsl = true;
                        NetworkCredential NetworkCred = new NetworkCredential("sysgww@gmail.com", "tsuna2000");
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = NetworkCred;
                        smtp.Port = 587;
                        smtp.Send(mm);
                        ViewBag.Message = "Email sent.";
                    }


                    db.ContentAssigns.Add(new ContentAssign
                    {
                        CTID = ct.CTID,
                        TopicID = id,
                        MCID = mcid
                    });

                }
            }
            else
            {
                TempData["message"] = "this topic is expired, Try another or wait for the next topic  ";
                return View();
            }
            db.SaveChanges();
            return RedirectToAction("Uploaded");

        }
        [Authorize(Roles = "Student,MarketingCoordinator,ManagerMarketing,Guest")]
        [HttpPost]
        public FileResult DownloadFile(int? fileId)
        {
            G5EnterpriseDBEntities entities = new G5EnterpriseDBEntities();
            Content file = entities.Contents.ToList().Find(p => p.CTID == fileId.Value);
            return File(file.Data, file.ContentType, file.Name);
        }
        

        [Authorize(Roles = "Student")]
        public ActionResult Edit(int id, string id1)
        {

            Content ct = db.Contents.Find(id);
            var student = (from s in db.Students
                           where s.UserName.Equals(User.Identity.Name)
                           select s).FirstOrDefault();

            ViewBag.StudentName = student.StudentName; //LINQ+ ENtityframework
            ViewBag.StudentID = student.StudentID;
            ViewBag.FacultyID = student.Faculty.FacultyName;
            ViewBag.TopicID = new SelectList(db.Topics.Where(g => g.FacultyID == student.Faculty.FacultyID).ToList(), "TopicID", "TopicName");
            return View(ct);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id1, HttpPostedFileBase postedImg, HttpPostedFileBase postedPDF, Content ct)
        {
            byte[] bytes;
            byte[] byte2s;
            using (BinaryReader br = new BinaryReader(postedImg.InputStream))
            {
                bytes = br.ReadBytes(postedImg.ContentLength);
            }
            using (BinaryReader br2 = new BinaryReader(postedPDF.InputStream))
            {
                byte2s = br2.ReadBytes(postedPDF.ContentLength);
            }

            var student = (from s in db.Students
                           where s.UserName.Equals(User.Identity.Name)
                           select s).FirstOrDefault();
            
            var tpid = (from t in db.Topics
                        where t.TopicID.Equals(ct.TopicID)
                        select t).FirstOrDefault();

            ViewBag.StudentName = student.StudentName;
            ViewBag.StudentID = student.StudentID;
            ViewBag.FacultyID = student.Faculty.FacultyID;
            ViewBag.TopicID = new SelectList(db.Topics.Where(g => g.FacultyID == student.Faculty.FacultyID).ToList(), "TopicID", "TopicName");
            
            if (DateTime.Now < (DateTime)tpid.EndDate)
            {
                if (ModelState.IsValid)
                {
                    ct.StudentID = ViewBag.StudentID;
                    ct.FacultyID = ViewBag.FacultyID;
                    ct.Name2 = Path.GetFileName(postedImg.FileName);
                    ct.ContentType2 = postedImg.ContentType;
                    ct.Data2 = bytes;
                    ct.Name = Path.GetFileName(postedPDF.FileName);
                    ct.Data = byte2s;
                    ct.ContentType = postedPDF.ContentType;
                    db.Entry(ct).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Uploaded");
                }
            }
            else
            {
                TempData["message"] = "You can't edit because this topic is expired ";
                return View();

            }
            return View(ct);
        }
        [Authorize(Roles = "Student")]
        public ActionResult Delete(int id)
        {
            ContentAssign ctas = db.ContentAssigns.Find(id);
            Content ct = db.Contents.Find(id);
            return View(ct);
        }
        [HttpPost, ActionName("Delete")]

        public ActionResult DeleteConfirmed(int id)
        {
            Content ct = db.Contents.Find(id);
            ContentAssign ctas = db.ContentAssigns.Find(id);
            db.ContentAssigns.Remove(ctas);
            db.Contents.Remove(ct);
            db.SaveChanges();
            return RedirectToAction("Uploaded");
        }

        public ActionResult Term_And_Conditions()
        {
            //Response.Write("<script>alert('wrong password or user dosent exist');</script>");
            return View();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [Authorize(Roles = "Student")]
        public ActionResult Details(int id)
        {
            Content ct = db.Contents.Find(id);
            if (ct == null)
            {
                return HttpNotFound();
            }
            return View(ct);
        }
    }
}