using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ArtSite.DataAccess;
using ArtSite.Models;
using ArtSite.Models.Views;

namespace ArtSite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(PictureEditorController.GetImagesForLandingPage());
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult LandingPageOld()
        {
            return View();
        }

        public ViewResult LandingPage()
        {
            return View();
        }


       
    }
}
