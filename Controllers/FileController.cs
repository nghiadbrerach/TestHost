using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.IO;


// FOLDER UPLOAD

namespace WebEnterprise.Controllers
{
    public class FileController : Controller
    {
        // GET: File
        public ActionResult Index()
        {
            List<ObjFile> ObjFiles = new List<ObjFile>();
            foreach (string strfile in Directory.GetFiles(Server.MapPath("~/Files")))
            {
                FileInfo fi = new FileInfo(strfile);
                ObjFile obj = new ObjFile();
                obj.File = fi.Name;
                obj.Size = fi.Length;
                obj.Type = GetFileTypeByExtension(fi.Extension);
                ObjFiles.Add(obj);
            }

            return View(ObjFiles);
        }

        public FileResult Download(string fileName)
        {
            string fullPath = Path.Combine(Server.MapPath("~/Files"), fileName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(fullPath);



            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        private string GetFileTypeByExtension(string fileExtension)
        {
            switch (fileExtension.ToLower())
            {
                case ".pdf":
                    return "Portable Document Format";
                case ".docx":
                case ".doc":
                    return "Microsoft Word Document";
                case ".xlsx":
                case ".xls":
                    return "Microsoft Excel Document";
                case ".txt":
                    return "Text Document";
                case ".jpg":
                case ".png":
               
                    return "Image";
             
                    
                default:
                    return "Unknown";

            }
        }
        [HttpPost]
        public ActionResult Index(ObjFile doc)
        {
            foreach (var file in doc.files)
            {

                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(Server.MapPath("~/Files"), fileName);
                    file.SaveAs(filePath);
                }
            }
            TempData["Message"] = "files uploaded successfully";
            return RedirectToAction("Index");
        }

    }
}
public class ObjFile
{
    public IEnumerable<HttpPostedFileBase> files { get; set; }
    public string File { get; set; }
    public long Size { get; set; }
    public string Type { get; set; }
}