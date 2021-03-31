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
    public class MarketingManagerController : Controller
    {
        private G5EnterpriseDBEntities db = new G5EnterpriseDBEntities();
        [Authorize(Roles = "Admin")]
        // GET: MarketingCoordinators
        public ActionResult Index()
        {
            var marketingmanager = db.MarketingManagers;
            return View(marketingmanager.ToList());
        }

        // GET: MarketingCoordinators/Details/5
        [Authorize(Roles = "ManagerMarketing,Admin")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MarketingManager marketingmanager = db.MarketingManagers.Find(id);
            if (marketingmanager == null)
            {
                return HttpNotFound();
            }
            return View(marketingmanager);
        }

        // GET: MarketingCoordinators/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {

            return View();
        }

        // POST: MarketingCoordinators/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(MarketingManager marketingmanager)
        {
            if (ModelState.IsValid)
            {
                db.MarketingManagers.Add(marketingmanager);
                db.SaveChanges();

                AuthenController.CreateAccount(marketingmanager.UserName, "123456", "ManagerMarketing");

                return RedirectToAction("Index", "Home");
            }


            return View(marketingmanager);
        }

        // GET: MarketingCoordinators/Edit/5
        [Authorize(Roles = "ManagerMarketing")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MarketingManager marketingmanager = db.MarketingManagers.Find(id);
            if (marketingmanager == null)
            {
                return HttpNotFound();
            }

            ViewBag.UName = (from s in db.MarketingManagers
                             where s.UserName == User.Identity.Name
                             select s.UserName).FirstOrDefault();

            return View(marketingmanager);
        }

        // POST: MarketingCoordinators/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(MarketingManager marketingmanager)
        {
            var a = (from s in db.MarketingManagers
                     where s.UserName == User.Identity.Name
                     select s.UserName).FirstOrDefault();
            if (ModelState.IsValid)
            {
                marketingmanager.UserName = a;
                db.Entry(marketingmanager).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }



            return View(marketingmanager);
        }

        // GET: MarketingCoordinators/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MarketingManager marketingmanager = db.MarketingManagers.Find(id);
            if (marketingmanager == null)
            {
                return HttpNotFound();
            }
            return View(marketingmanager);
        }

        // POST: MarketingCoordinators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeleteConfirmed(string id)
        {
            MarketingManager marketingmanager = db.MarketingManagers.Find(id);
            db.MarketingManagers.Remove(marketingmanager);
            db.SaveChanges();
            AuthenController.DeleteAccount(marketingmanager.UserName);
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
        [Authorize(Roles = "ManagerMarketing")]
        public ActionResult ViewStatic()
        { //Number of contributions within each Faculty for each academic year.
            var a = (from s in db.Contents
                     where s.Faculty.FacultyName.Equals("IT")
                     select s).Count();
            ViewBag.IT = a;
            var b = (from s in db.Contents
                     where s.Faculty.FacultyName.Equals("Business")
                     select s).Count();
            ViewBag.Business = b;
            var c = (from s in db.Contents
                     where s.Faculty.FacultyName.Equals("Design")
                     select s).Count();
            ViewBag.Design = c;
            var d = (from s in db.Contents
                     where s.Faculty.FacultyName.Equals("Sport")
                     select s).Count();
            ViewBag.Sport = d;
            var e = db.Contents.Count();
            ViewBag.Total = e;

            //Percentage of contributions by each Faculty for any academic year.

            ViewBag.PerIT = Math.Round((float)a / e * 100, 2) + "%";
            ViewBag.PerBusiness = Math.Round((float)b / e * 100, 2) + "%";
            ViewBag.PerDesign = Math.Round((float)c / e * 100, 2) + "%";
            ViewBag.PerSport = Math.Round((float)d / e * 100, 2) + "%";

            //Number of contributors within each Faculty for each academic year.

            var f = (from s in db.Contents where s.Faculty.FacultyName.Equals("IT") select s.StudentID).Distinct().Count();
            ViewBag.StdIT = f;
            var g = (from s in db.Contents where s.Faculty.FacultyName.Equals("Business") select s.StudentID).Distinct().Count();
            ViewBag.StdBusiness = g;
            var h = (from s in db.Contents where s.Faculty.FacultyName.Equals("Design") select s.StudentID).Distinct().Count();
            ViewBag.StdDesign = h;
            var y = (from s in db.Contents where s.Faculty.FacultyName.Equals("Sport") select s.StudentID).Distinct().Count();
            ViewBag.StdSport = y;
            ViewBag.TotalCTor = (f + g + h + y);

            //Percentage of contributors
            var k = (from s in db.Students where s.Faculty.FacultyName.Equals("IT") select s.StudentID).Count();
            ViewBag.CotIT = k;
            var l = (from s in db.Students where s.Faculty.FacultyName.Equals("Business") select s.StudentID).Count();
            ViewBag.CotBusiness = l;
            var m = (from s in db.Students where s.Faculty.FacultyName.Equals("Design") select s.StudentID).Count();
            ViewBag.CotDesign = m;
            var n = (from s in db.Students where s.Faculty.FacultyName.Equals("Sport") select s.StudentID).Count();
            ViewBag.CotSport = n;


            var z = Math.Round((float)f / k * 100, 2);
            ViewBag.perStdIT = z;
            var u = Math.Round((float)g / k * 100, 2);
            ViewBag.perStdBusiness = u;
            var p = Math.Round((float)h / k * 100, 2);
            ViewBag.perStdDesign = p;
            var q = Math.Round((float)y / k * 100, 2);
            ViewBag.perStdSport = q;
            return View();
        }
        [Authorize(Roles = "ManagerMarketing")]
        public ActionResult ExceptionalReports()
        {
            var exceptionalReports = db.ContentAssigns
                .Where(s => s.CommentA.Equals(null))
                .ToList();
            return View(exceptionalReports.ToList());
        }
        [Authorize(Roles = "ManagerMarketing")]
        public ActionResult SelectedCT()
        {
            var AcceptContent = db.ContentAssigns
                .Where(s => s.Status.GiveStatus.Contains("Accept"))
                .ToList();
            
            return View(AcceptContent.ToList());
        }
        public ActionResult Search(string searchString)
        {
            var ctsearch = from m in db.ContentAssigns select m;
            if(!String.IsNullOrEmpty(searchString))
            {
                ctsearch = ctsearch.Where(s => s.Content.CTName.Contains(searchString) && s.Status.GiveStatus.Contains("Accept"));
            }
            else
            {
                return RedirectToAction("SelectedCT");
            }
            return View(ctsearch);
            
        }
        public ActionResult MMProfile()
        {
            var UserName = User.Identity.Name;
            var userId = UserName;

            var MM = (from a in db.MarketingManagers where a.UserName.ToString().Equals(userId) select a).FirstOrDefault();
            return View(MM);
        }
    }
}
