using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ArtSite.DataAccess;
using ArtSite.Filters;
using ArtSite.Models;
using ArtSite.Models.Views;
using MultipleUploads;

namespace ArtSite.Controllers
{
    public class NewsController : Controller
    {
        public ActionResult Index()
        {
            NewsItem news    = null;
            using(var artGalleryDbContext = new ArtGalleryDBContext())
            {
                news = artGalleryDbContext.NewsItem.FirstOrDefault();
            }
            return View(news);
        }

        [AdminPermissions]
        public ViewResult Edit()
        {
            return View();
        }

        [HttpPost, AdminPermissions]
        public ActionResult Edit(NewsItem newsItem, HttpPostedFileBase file)
        {

            var filePath = HttpContext.Server.MapPath("../MyFiles");

            var fullPath = Path.Combine(filePath, "news.jpg");
            
            if(file!=null)
                ImageProcessor.ResizeAndSaveImage(400,400, fullPath, file.InputStream);
            else
            {
                //delete news image
                if(System.IO.File.Exists(fullPath))
                    System.IO.File.Delete(fullPath);
            }

            using (var artGalleryDbContext = new ArtGalleryDBContext())
            {
                var currentNews = artGalleryDbContext.NewsItem.FirstOrDefault();
                if (currentNews != null)
                {
                    currentNews.Body = newsItem.Body;
                    currentNews.Subject = newsItem.Subject;
                }
                else
                    artGalleryDbContext.NewsItem.Add(newsItem);

                artGalleryDbContext.SaveChanges();
            }

            return RedirectToAction("Index");
            //news.Image = new byte[file.InputStream.Length];
            //file.InputStream.Rea
        }

       
    }
}
