using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebEnterprise.Models;

namespace WebEnterprise.Controllers
{
    public class GuestController : Controller
    {
        private G5EnterpriseDBEntities db = new G5EnterpriseDBEntities();

        // GET: Guest
        public ActionResult Index()
        {
            return View(db.Guests.ToList());
        }
        [Authorize(Roles = "Admin,Guest")]
        public ActionResult Details(int id)
        {
            Guest guest = db.Guests.Find(id);
            return View(guest);
        }
        [Authorize(Roles = "Admin")]
        // GET: Students/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(Guest guest)
        {
            if (ModelState.IsValid)
            {
                db.Guests.Add(guest);
                db.SaveChanges();

                AuthenController.CreateAccount(guest.UserName, "123456", "Guest");

                return RedirectToAction("Index");
            }

            return View(guest);
        }
        [Authorize(Roles = "Guest,Admin")]
        public ActionResult Edit(int id)
        {
            Guest guest = db.Guests.Find(id);
            return View(guest);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(Guest guest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(guest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(guest);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            Guest guest = db.Guests.Find(id);
            return View(guest);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeleteConfirmed(int id)
        {
            Guest guest = db.Guests.Find(id);
            db.Guests.Remove(guest);
            db.SaveChanges();
            AuthenController.DeleteAccount(guest.UserName);
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