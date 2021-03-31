using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebEnterprise.Models;



// DATABASE UPLOAD


namespace WebEnterprise.Controllers
{
    [RoutePrefix("Pdf")]
    public class FileUploadController : Controller
    {
        private G5EnterpriseDBEntities db = new G5EnterpriseDBEntities();
        // GET: FileUpload

        public ActionResult Index()
        {

            return View(db.tblFiles.ToList());
        }
        
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase postedFile)
        {
            
            byte[] bytes;
            using (BinaryReader br = new BinaryReader(postedFile.InputStream))
            {
                bytes = br.ReadBytes(postedFile.ContentLength);
            }

            db.tblFiles.Add(new tblFile
            {                  
                Name = Path.GetFileName(postedFile.FileName),
                ContentType = postedFile.ContentType,
                Data = bytes
            });
            
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        [HttpPost]
        public FileResult DownloadFile(int? fileId)
        {

            tblFile file = db.tblFiles.ToList().Find(p => p.ID == fileId.Value);
            return File(file.Data, file.ContentType, file.Name);
        }
        public ActionResult View()
        {

            return View(db.tblFiles.ToList());
        }



    }
}