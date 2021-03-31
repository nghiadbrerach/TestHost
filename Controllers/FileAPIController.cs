using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebEnterprise.Models;
using System.IO;
using System.Web;

using System.Net.Http.Headers;
namespace WebEnterprise.Controllers
{    
   
    public class FileAPIController : ApiController
    {
         private G5EnterpriseDBEntities3 db = new G5EnterpriseDBEntities3();
        [HttpPost]
        [Route("FileAPI/SaveFile")]
        public HttpResponseMessage SaveFile()
        {
            //Create HTTP Response.
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

            //Check if Request contains File.
            if (HttpContext.Current.Request.Files.Count == 0)
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            //Read the File data from Request.Form collection.
            HttpPostedFile postedFile = HttpContext.Current.Request.Files[0];

            //Convert the File data to Byte Array.
            byte[] bytes;
            using (BinaryReader br = new BinaryReader(postedFile.InputStream))
            {
                bytes = br.ReadBytes(postedFile.ContentLength);
            }

            //Insert the File to Database Table.
            
            tblFile file = new tblFile
            {
                Name = Path.GetFileName(postedFile.FileName),
                ContentType = postedFile.ContentType,
                Data = bytes
            };
            db.tblFiles.Add(file);
            db.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK, new { id = file.ID, Name = file.Name });
        }

        [HttpPost]
        [Route("FileAPI/SaveFile")]
        public HttpResponseMessage GetFiles()
        {
            
            var files = from file in db.tblFiles
                        select new { id = file.ID, Name = file.Name };
            return Request.CreateResponse(HttpStatusCode.OK, files);
        }

        [HttpGet]
        [Route("FileAPI/SaveFile")]
        public HttpResponseMessage GetFile(int fileId)
        {
            //Create HTTP Response.
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

            //Fetch the File data from Database.
           
            tblFile file = db.tblFiles.ToList().Find(p => p.ID == fileId);

            //Set the Response Content.
            response.Content = new ByteArrayContent(file.Data);

            //Set the Response Content Length.
            response.Content.Headers.ContentLength = file.Data.LongLength;

            //Set the Content Disposition Header Value and FileName.
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = file.Name;

            //Set the File Content Type.
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            return response;
        }
    }
}
