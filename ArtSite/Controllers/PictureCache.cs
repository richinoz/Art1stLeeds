using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using ArtSite.DataAccess;
using ArtSite.Models;
using MultipleUploads;

namespace ArtSite.Controllers
{
    public static class PictureCache
    {
        private static readonly object _lockObject = new object();

        static PictureCache()
        {
            Refresh();
        }


        public static void Refresh()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path).Replace("/", @"\");

            path = path.Substring(0, path.LastIndexOf(@"\"));
            path = path.Substring(0, path.LastIndexOf(@"\")) + "\\MyFiles\\";

            Refresh(path);
        }
        public static void Refresh(string filePath, bool deleteFiles = false)
        {
            try
            {
                lock (_lockObject)
                {
                    using (var db = new ArtGalleryDBContext())
                    {
                        var pics = PictureEditorController.GetImagesFromDbMin("");

                        IPictureDal pictureDal = new PictureDal(db);


                        //delete all disk files
                        if (filePath != null && deleteFiles)
                        {

                            var files = Directory.GetFiles(filePath);
                            foreach (var file in files)
                            {
                                File.Delete(file);
                            }


                            //re-write images
                            foreach (var pic in pics)
                            {

                                string fullImagePath = filePath + pic.ID + ".jpg";
                                string smallImagePath = filePath + pic.ID + "_s.jpg";

                                var picWithData = pictureDal.Enitities.Find(pic.ID);
                                ImageProcessor.ResizeAndSaveImage(1280, 720, fullImagePath, picWithData.ImageT);

                                var pBuffer = ImageProcessor.CreateThumbnail(fullImagePath, 150, 150, ".jpg");
                                ImageProcessor.ResizeAndSaveImage(150, 150, smallImagePath, pBuffer);

                            }
                        }
                        else
                        {

                            //re-write images
                            foreach (var pic in pics)
                            {

                                string fullImagePath = filePath + pic.ID + ".jpg";
                                string smallImagePath = filePath + pic.ID + "_s.jpg";

                                if (!File.Exists(fullImagePath))
                                {
                                    var picWithData = pictureDal.Enitities.Find(pic.ID);
                                    ImageProcessor.ResizeAndSaveImage(1280, 720, fullImagePath, picWithData.ImageT);

                                    var pBuffer = ImageProcessor.CreateThumbnail(fullImagePath, 150, 150, ".jpg");
                                    ImageProcessor.ResizeAndSaveImage(150, 150, smallImagePath, pBuffer);
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("in Refresh cache", ex);
            }
        }

        public static Bitmap GetImage(string path, long screenWidth, long screenHeight, long picId)
        {
            Bitmap bitmap = null;
            byte[] pBuffer = null;

            string fullImagePath = path + ".jpg";
            string smallImagePath = path + "_s.jpg";
            //only retrieve from db if not on disk
            if (File.Exists(fullImagePath))
            {

                if (File.Exists(smallImagePath))
                {
                    var img = System.Drawing.Image.FromFile(smallImagePath);
                    bitmap = new Bitmap(img);
                }
                else
                {
                    pBuffer = ImageProcessor.CreateThumbnail(fullImagePath, 150, 150, "jpg");
                    ImageProcessor.ResizeAndSaveImage(150, 150, smallImagePath, pBuffer);
                }

            }
            else
            {
                using (ArtGalleryDBContext db = new ArtGalleryDBContext())
                {
                    var pic = db.Pictures.Find(picId);
                    ImageProcessor.ResizeAndSaveImage(screenWidth, screenHeight, fullImagePath, pic.ImageT);

                    pBuffer = ImageProcessor.CreateThumbnail(fullImagePath, 150, 150, ".jpg");
                    ImageProcessor.ResizeAndSaveImage(150, 150, smallImagePath, pBuffer);
                }
            }

            // Convert Byte[] to Bitmap
            if (bitmap == null)
                bitmap = ConvertToBitmap(pBuffer);

            return bitmap;
        }

        // Convert byte array to Bitmap (byte[] to Bitmap)
        private static Bitmap ConvertToBitmap(byte[] bmp)
        {
            if (bmp != null)
            {
                TypeConverter tc = TypeDescriptor.GetConverter(typeof(Bitmap));
                Bitmap b = (Bitmap)tc.ConvertFrom(bmp);
                return b;
            }
            return null;
        }
    }
}