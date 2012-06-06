using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtSite.Controllers
{
    public class PUploaderController : Controller
    {
        //
        // GET: /PUploader/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Html5Uploader()
        {
            return View();
        }

    }
}
