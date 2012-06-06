using System;
using System.Configuration;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.ComponentModel;
using MultipleUploads;

namespace ArtSite
{    
    public class ShowImageFromDisk : IHttpHandler
    {
        long seq = 0;
        byte[] empPic = null;

        public void ProcessRequest(HttpContext context)
        {
            string filePath;
            if (context.Request.QueryString["id"] != null)
            {
                filePath = (context.Request.QueryString["id"]);
                filePath = HttpUtility.UrlDecode(filePath);
                filePath = HttpContext.Current.Server.MapPath(filePath);
            }
            else
                throw new ArgumentException("No parameter specified");            

            byte[] pBuffer = ImageProcessor.CreateThumbnail(filePath, 150, 150, ".jpg");

            // Convert Byte[] to Bitmap
            Bitmap newBmp = ConvertToBitmap(pBuffer);
            if (newBmp != null)
            {
                newBmp.Save(context.Response.OutputStream, ImageFormat.Jpeg);
                newBmp.Dispose();
            }
            else
            {

                Image img = System.Drawing.Image.FromFile(filePath);

                img.Save(context.Response.OutputStream, ImageFormat.Jpeg);
                img.Dispose();
            }
            
        }

        // Convert byte array to Bitmap (byte[] to Bitmap)
        protected Bitmap ConvertToBitmap(byte[] bmp)
        {
            if (bmp != null)
            {
                TypeConverter tc = TypeDescriptor.GetConverter(typeof(Bitmap));
                Bitmap b = (Bitmap)tc.ConvertFrom(bmp);
                return b;
            }
            return null;
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