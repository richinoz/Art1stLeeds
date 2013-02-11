using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ArtSite.DataAccess;
using MultipleUploads;
using ArtSite.Models;

namespace ArtSite.Controllers
{
    [Authorize]
    public class UploaderController : Controller
    {
        private ArtGalleryDBContext _db = new ArtGalleryDBContext();
        private IPictureDal _pictureDal;

        public UploaderController()
        {
            _pictureDal = new PictureDal(_db);
        }
        //
        // GET: /Uploader/
      
        public ActionResult Index()
        {            
            if (!Request.IsAuthenticated)
            {
                RouteValueDictionary dictionary=null;
                if(Request.Url!=null)
                    dictionary = new RouteValueDictionary(new {  returnUrl= Request.Url.PathAndQuery });
               
                return RedirectToAction("LogOn", "Account", dictionary);
            }

            ViewBag.Message = "You can upload .jpg or .jpeg files here";

            return View();
        }

        public ActionResult AllLoaders()
        {
            return View();
        }
        // This action handles the form POST and the upload
//        [HttpPost]
//        public ActionResult Index(HttpPostedFileBase file)
//        {
//            HttpFileCollectionBase hfc = null;
//            if (Session["UploadFiles"] != null)
//            {
//                hfc = (HttpFileCollectionBase)Session["UploadFiles"];
//                Session.Add("UploadFiles", null);
//            }
//            else
//            {
//                hfc = Request.Files;
//            }

//            List<string> filesUploaded = new List<string>();
//            try
//            {
//                // Get the HttpFileCollection

//                for (int i = 0; i < hfc.Count; i++)
//                {
//                    HttpPostedFileBase hpf = hfc[i];
//                    if (hpf.ContentLength > 0 && FileIsPicture(hpf))
//                    {
             

//                        byte[] buffer = ImageProcessor.ImageAsByteArray(hpf.InputStream, System.Drawing.Imaging.ImageFormat.Jpeg);

//                        PictureItem picture = new PictureItem {ImageT = buffer};


//                        _pictureDal.Enitities.Add(picture);
//                        _pictureDal.SaveChanges();

//                        filesUploaded.Add(hpf.FileName);

//                    }
//                }

//                PictureCache.Refresh();
//            }
//            catch (Exception ex)
//            {
//#if DEBUG
//                throw;
//#endif
//            }

//            return View("FilesUploaded", filesUploaded);
        
//        }

        public ActionResult FilesUploaded()
        {
            return View();
        }

        private static bool FileIsPicture(HttpPostedFileBase file)
        {
            string fileName = file.FileName.ToLower();

            return fileName.EndsWith(".jpg") || fileName.EndsWith(".jpeg");
        }

        [HttpPost]
        public ActionResult RefreshCache(int testParam1)
        {
            PictureCache.Refresh(Server.MapPath("..\\MyFiles\\"));
            return Json("success");
        }
        /// <summary>
        /// non HTTPPost
        /// </summary>
        /// <returns></returns>
        public ActionResult RefreshCache()
        {
            PictureCache.Refresh(Server.MapPath("..\\MyFiles\\"), false);
            return Json("success");
        }

        public ActionResult ForceCache()
        {
            PictureCache.Refresh(Server.MapPath("..\\MyFiles\\"), true);
            return Json("success");
        }
    }

}
