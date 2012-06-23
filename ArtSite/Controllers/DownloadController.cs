using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ionic.Zip;

namespace ArtSite.Controllers
{
    public class DownloadController : Controller
    {
        //
        // GET: /Download/

        public ActionResult Index(string fileName)
        {

            string file = Server.MapPath("~/MyFiles/" + fileName);
            if (System.IO.File.Exists(file))
            {
                var zipFile = Zip(file);
                return File(zipFile, "zip", "downloadTest.zip");
            }
            else
                return null;
        }

        private string Zip(string path)
        {
            string dest = Server.MapPath("~/MyFiles/" + "downLoad.zip");


            using (var zip = new ZipFile())
            {
                // add this map file into the "images" directory in the zip archive
                zip.AddFile(path, "images");
                //zip.AddFile(path, "images2");
                // add the report into a different directory in the archive
                // zip.AddFile("c:\\Reports\\2008-Regional-Sales-Report.pdf", "files");
               // zip.AddFile(path);
                zip.Save(dest);
            }

            return dest;
        }


    }
}
