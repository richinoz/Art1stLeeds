using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Web;

namespace MultipleUploads
{
    public class ImageProcessor
    {       
        public static byte[] ImageAsByteArray(string fileName, System.Drawing.Imaging.ImageFormat format)
        {
            byte[] buffer;
            Image image = Image.FromFile(fileName);
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, format);
                    buffer = ms.ToArray();
                }
            }
            finally
            {
                image.Dispose();
            }
            return buffer;
        }

        public static byte[] ImageAsByteArray(Stream inputFileStream, System.Drawing.Imaging.ImageFormat format)
        {
            byte[] buffer=null;
            try
            {
                using (var image = Image.FromStream(inputFileStream))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        image.Save(ms, format);
                        buffer = ms.ToArray();
                    }
                }
            }catch{}
            
            return buffer;
        }
      

        public static byte[] CreateThumbnail(string fileName, int maxHeight, int maxWidth, string extension)
        {
            Image img = System.Drawing.Image.FromFile(fileName);
            return CreateThumbnail(img, maxHeight, maxWidth, extension);
        }

      

        /// Create thumbnail of the image
        public static byte[] CreateThumbnail(Image img, int maxHeight, int maxWidth, string extension)
        {
            
            extension = extension.ToLower();

            byte[] buffer = null;
            try
            {
                int width = img.Size.Width;
                int height = img.Size.Height;

                bool doWidthResize = (maxWidth > 0 && width > maxWidth && width > maxHeight);
                bool doHeightResize = (maxHeight > 0 && height > maxHeight && height > maxWidth);

                //only resize if the image is bigger than the max
                if (doWidthResize || doHeightResize)
                {
                    int iStart;
                    Decimal divider;
                    if (doWidthResize)
                    {
                        iStart = width;
                        divider = Math.Abs((Decimal)iStart / (Decimal)maxWidth);
                        width = maxWidth;
                        height = (int)Math.Round((Decimal)(height / divider));
                    }
                    else
                    {
                        iStart = height;
                        divider = Math.Abs((Decimal)iStart / (Decimal)maxHeight);
                        height = maxHeight;
                        width = (int)Math.Round((Decimal)(width / divider));
                    }
                    System.Drawing.Image newImg = img.GetThumbnailImage(width, height, null, new System.IntPtr());
                    try
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            if (extension.IndexOf("jpg") > -1)
                            {
                                newImg.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                            }
                            else if (extension.IndexOf("png") > -1)
                            {
                                newImg.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                            }
                            else if (extension.IndexOf("gif") > -1)
                            {
                                newImg.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                            }
                            else // if (extension.IndexOf("bmp") > -1)
                            {
                                newImg.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                            }
                            buffer = ms.ToArray();
                        }
                    }
                    finally
                    {
                        newImg.Dispose();
                    }
                }
            }
            catch
            {
                System.Drawing.Image imNoimage = System.Drawing.Image.FromFile("/images/noimage.gif");
                try
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        imNoimage.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                        buffer = ms.ToArray();
                    }
                }
                finally
                {
                    imNoimage.Dispose();
                }
            }
            finally
            {
                img.Dispose();
            }
            return buffer;
        }

        public static void ResizeAndSaveImage(long maxWidth, long maxHeight, string targetPath, Stream stream)
        {
            using (var image = Image.FromStream(stream))
            {
                double scaleFactorW = (double) maxWidth/image.Width;
                double scaleFactorH = (double) maxHeight/image.Height;

                double scaleFactor = Math.Min(scaleFactorH, scaleFactorW);


                if (scaleFactor < 1)
                {
                    // can given width of image as we want
                    var newWidth = (int) (image.Width*scaleFactor);
                    // can given height of image as we want
                    var newHeight = (int) (image.Height*scaleFactor);
                    var thumbnailImg = new Bitmap(newWidth, newHeight);
                    var thumbGraph = Graphics.FromImage(thumbnailImg);
                    thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                    thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                    thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                    thumbGraph.DrawImage(image, imageRectangle);
                    thumbnailImg.Save(targetPath, image.RawFormat);

                    thumbnailImg.Dispose();
                    thumbGraph.Dispose();
                    
                }
                else
                {
                    image.Save(targetPath, image.RawFormat);
                }
                image.Dispose();
            }
        }


        public static void ResizeAndSaveImage(long maxWidth, long maxHeight, string targetPath, byte[] buffer)
        {

            using (Stream stream = new MemoryStream(buffer))
            {
                ResizeAndSaveImage(maxWidth, maxHeight, targetPath, stream);
                stream.Dispose();
            }
        }

    }
}