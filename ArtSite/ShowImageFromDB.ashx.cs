using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Drawing2D;
using System.IO;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.ComponentModel;
using ArtSite.Controllers;
using ArtSite.Models;
using MultipleUploads;

namespace ArtSite
{
    public class ShowImageFromDb : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
               

                long id;
                long screenWidth = 1280;
                long screenHeight = 720;

                if (context.Request.QueryString["id"] != null)
                    id = (Convert.ToInt64(context.Request.QueryString["id"]));
                else
                    throw new ArgumentException("No parameter specified");

                //if (context.Request.QueryString["screenWidth"] != null)
                //    screenWidth = (Convert.ToInt64(context.Request.QueryString["screenWidth"]));

                //if (context.Request.QueryString["screenHeight"] != null)
                //    screenHeight = (Convert.ToInt64(context.Request.QueryString["screenHeight"]));    

                //var pic = PictureCache.Pictures.Find(x=>x.ID == id);

                //if (pic.ImageT == null)
                //    return;
                ////save this so that lightbox can reference it!!
                var path = context.Server.MapPath("MyFiles") + "\\" + System.IO.Path.GetFileName(id.ToString());

                
                //refresh images once a day to prevent dodgy caching in browsers?

                var bitmap = PictureCache.GetImage(path, screenWidth, screenHeight, id);

                if (bitmap != null)
                {
                    bitmap.Save(context.Response.OutputStream, ImageFormat.Jpeg);
                    bitmap.Dispose();
                }

            }
            catch (Exception ex)
            {
                Logger.Error("ShowImageFromDb", ex);
            }

        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }


    }

}