using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SuperFish.Models;

namespace SuperFish.Controllers
{
    public class PictureLoaderController : Controller
    {
        private ArtGalleryDBContext _db = new ArtGalleryDBContext();
        public ActionResult Index(string name)
        {
            return View(GetImagesFromDb(name));
        }

        public ActionResult VerticalLoader()
        {
            return View(GetImagesFromDb());
        }

        public ActionResult VerticalLoaderLightBox1()
        {
            return View(GetImagesFromDb());
        }

        internal IEnumerable<ImageWrapper> GetImagesFromDb()
        {
            return GetImagesFromDb(null);
        }

        public IEnumerable<ImageWrapper> GetImagesFromDb(string searchName)
        {
            List<ImageWrapper> images = new List<ImageWrapper>();
            foreach(var item in _db.Pictures)
            {                
                bool add = false;

                if (!string.IsNullOrEmpty(searchName))
                {
                    if (item.Title != null)
                        if (item.Title.ToLower().Contains(searchName.ToLower()))
                            add = true;                        
                    
                    if (item.Artist != null)
                        if (item.Artist.ToLower().Contains(searchName.ToLower()))
                            add = true;
                }
                else
                    add = true;

                if (add)
                    images.Add(new ImageWrapper(String.Format("/MyFiles/{0}", Path.GetFileName(item.ID.ToString())), item.Title));
            }

            return images;
        }
        //public IEnumerable<ImageWrapper> BindGrid(string searchName)
        //{
        //    string[] filesindirectory = Directory.GetFiles(Server.MapPath("~/MyFiles"), "*.jp*");
        //    List<ImageWrapper> images = new List<ImageWrapper>();
                      
        //    foreach (string item in filesindirectory)
        //    {
        //        bool add = true;
        //        if (searchName != null)
        //        {
        //            FileInfo fileInfo = new FileInfo(item);
        //            if (!fileInfo.Name.ToLower().Contains(searchName.ToLower()))
        //                add = false;
        //        }
        //        if(add)
        //            images.Add(new ImageWrapper(String.Format("/MyFiles/{0}", System.IO.Path.GetFileName(item))));
        //    }

        //    return images;
        //}

    }
}
