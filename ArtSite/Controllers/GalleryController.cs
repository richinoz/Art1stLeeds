﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ArtSite.DataAccess;
using ArtSite.Extensions;
using ArtSite.Models;
using ArtSite.Models.Views;

namespace ArtSite.Controllers
{ 
    public class GalleryController : Controller
    {
        private ArtGalleryDBContext _db = new ArtGalleryDBContext();
        // GET: /Gallery/
       
        public ViewResult Index(string name)
        {
            HttpCookie httpCookie = Request.Cookies["ImagesPerPage"];
            if (httpCookie != null)
                ViewBag.ItemsPerPage = httpCookie.Value;

            if(name!=null)
                return View(PictureEditorController.GetImagesFromDbMin(name));

            return View(PictureEditorController.GetImagesFromDbMin(""));
        }

        public ViewResult ArtistHome(long userId)
        {
            var artist = Helpers.Helpers.GetUserForId(userId, _db);
            var artistId = artist != null ? artist.UserId : 0;
            var name = artist != null ? artist.DisplayNameText : string.Empty;

            ViewBag.ArtistId = artistId;
            ViewBag.ArtistName = name;
           
            ViewBag.Menu = "_ArtistHome";
            ViewBag.Model = artist;
            ViewBag.Title = name;

            return View(artist);        
            
        }

        public ViewResult ArtistHomeByName(string artistName)
        {
            var artist = Helpers.Helpers.GetUserForName(artistName, _db);
            var artistId = artist != null ? artist.UserId : 0;
            var name = artist != null ? artist.DisplayNameText : string.Empty;

            ViewBag.ArtistId = artistId;
            ViewBag.ArtistName = name;

            ViewBag.Menu = "_ArtistHome";
            ViewBag.Model = artist;
            ViewBag.Title = name;

            return View("ArtistHome", artist);

        }

        public ActionResult Searching(string name, string items)
        {
            HttpCookie httpCookie = Response.Cookies["ImagesPerPage"];

            if (httpCookie == null)
            {
                httpCookie = new HttpCookie("ImagesPerPage");
                httpCookie.Expires = DateTime.Now.AddDays(20);
                Response.Cookies.Add(httpCookie);
            }
            
            httpCookie.Value = items;
            Response.Cookies.Set(httpCookie);
            
            ViewBag.StartIndex = 0;

            var images = PictureEditorController.GetImagesFromDbMin(name);

            //store model to session - no need for reload from db
            Session["model"] = images;

            if (Request.IsAjaxRequest())
                return PartialView("Searching", images);

            return View(PictureEditorController.GetImagesFromDbMin(name));
        }

        public ViewResult ArtistGallery(long userId, string theme)
        {
            var artist = Helpers.Helpers.GetUserForId(userId, _db);
            var artistId = artist != null ? artist.UserId : 0;
            var name = artist != null ? artist.DisplayNameText : string.Empty;

            string themeVal = null;
            if (theme!=null)
                themeVal = " - " + theme.ToTitleCase();
            
            ViewBag.ArtistId = artistId;
            ViewBag.ArtistName = name;
            ViewBag.Title = string.Format("{0}{1}", name, themeVal);
            ViewBag.Model = artist;
            ViewBag.Menu = "_ArtistHome";

            if (theme == "other")
                theme = String.Empty;

            var artistGalleryViewModel = new ArtistGalleryViewModel
                                             {
                                                 Pictures = (PictureEditorController.GetImagesFromDbMin(artist, theme)),
                                                 Artist = artist,
                                                 Theme = theme
                                             };


            if (artistGalleryViewModel.Categories != null && theme != null)
                artistGalleryViewModel.Pictures = artistGalleryViewModel.Categories.FirstOrDefault(x => x.Key.ToLower() == theme).Value;

            return View(artistGalleryViewModel);
        }

   
        public ViewResult ArtistThemes(long userId)
        {
            var artist = Helpers.Helpers.GetUserForId(userId, _db);
            var artistId = artist != null ? artist.UserId : 0;
            var name = artist != null ? artist.DisplayNameText : string.Empty;

            ViewBag.ArtistId = artistId;
            ViewBag.ArtistName = name;
            ViewBag.Title = string.Format("{0} - {1}", name, "Themes");
            ViewBag.Model = artist;
            ViewBag.Menu = "_ArtistHome";


            var artistGalleryViewModel = new ArtistGalleryViewModel()
            {
                Pictures = (PictureEditorController.GetImagesFromDbMin(artist)),
                Artist = artist,
                Categories = new Dictionary<string, List<PictureItemNoBufferData>>()
            };

            foreach (var picture in artistGalleryViewModel.Pictures)
            {
                var theme = string.IsNullOrWhiteSpace(picture.Theme) ? "other" : picture.Theme.ToTitleCase().Trim();

                if (!artistGalleryViewModel.Categories.ContainsKey(theme))
                    artistGalleryViewModel.Categories.Add(theme, new List<PictureItemNoBufferData>());

                artistGalleryViewModel.Categories[theme].Add(picture);
            }

            return View(artistGalleryViewModel);
        }

        [Authorize]
        public ViewResult LandingPagePictureChooser(int landPageItemId)
        {

            var artist = Permissions.GetCurrentUser();

            var artistId = artist.UserId;
            var name = artist.DisplayNameText;

            ViewBag.ArtistId = artistId;
            ViewBag.ArtistName = name;
            ViewBag.Title = name;
            ViewBag.Model = artist;
            ViewBag.Menu = "_ArtistHome";

            var landingPageChooserViewModel = new LandingPageChooserViewModel()
            {
                Pictures = (PictureEditorController.GetImagesFromDbMin("")),
                LandingPageItemId = landPageItemId
            };
            return View(landingPageChooserViewModel);
        }

        [Authorize]
        public ActionResult LandingPagePictureReplacer(int landPageItemId, int newPictureId)
        {
            using(var artDb = new ArtGalleryDBContext())
            {
                var landingPageDal = new LandingPageDal(artDb);
                var landingPageItem = landingPageDal.Enitities.FirstOrDefault(x => x.Id == landPageItemId);
                landingPageItem.PictureId = newPictureId;
                artDb.SaveChanges();

            }
            return RedirectToAction("Index", "Home");
        }
        
        public ActionResult SearchingContinue(long index)
        {
            Response.Cookies.Set(Request.Cookies["ImagesPerPage"]);

            ViewBag.StartIndex = index;

            List<ArtSite.Models.PictureItemNoBufferData> model = (List<ArtSite.Models.PictureItemNoBufferData>)Session["model"];

            if (Request.IsAjaxRequest())
                return PartialView("Searching", model);

            return RedirectToAction("Searching", model);
        }   
      
        public ActionResult Gallerific()
        {
            return View(PictureEditorController.GetImagesFromDbMin(""));
        }

        public ActionResult Galleria(long? artistId)
        {
            if (artistId != null)
            {
                var artist = UserDal.AllUsers.FirstOrDefault(x => x.UserId == artistId.Value);
                if (artist != null)
                {
                    ViewBag.Model = artist;
                    ViewBag.Menu = "_ArtistHome";
                    return View("GalleriaforArtist", PictureEditorController.GetImagesFromDbMin(artist));
                }
            }
            return View(PictureEditorController.GetImagesFromDbMin(""));
        }

        [HttpPost]
        public ActionResult FileExists(string path)
        {
            string filepath = (Server.MapPath("..\\MyFiles\\" + path + "_s.jpg"));
            var f = System.IO.File.Exists(filepath);
            return Json(f.ToString());
        } 

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);            
        }
    }
}