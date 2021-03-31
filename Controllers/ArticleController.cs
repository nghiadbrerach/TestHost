using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

using System.Net.Http.Headers;

namespace WebEnterprise.Controllers
{
    public class ArticleController : Controller
    {
        // GET: Artical
        public ActionResult SubmitArticle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    MemoryStream target = new MemoryStream();
                    file.InputStream.CopyTo(target);
                    byte[] Bytes = target.ToArray();


                    file.InputStream.Read(Bytes, 0, Bytes.Length);
                    var fileContent = new ByteArrayContent(Bytes);
                    fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { FileName = file.FileName };


                    content.Add(fileContent);

                    content.Add(new StringContent("123"), "FileId");
                    //content.Headers.Add("Key", "abc23sdflsdf");
                    var requestUri = "https://localhost:44341/api/values";
                    var result = client.PostAsync(requestUri, content).Result;
                    if (result.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        ViewBag.Success = result.ReasonPhrase;

                    }
                    else
                    {
                        ViewBag.Failed = "Failed !" + result.Content.ToString();
                    }
                }
            }


            return View();
        }
    }
}