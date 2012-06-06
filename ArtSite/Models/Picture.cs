using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace ArtSite.Models
{
    public class PictureItem
    {
        public PictureItem()
        {  }
        
        public PictureItem(PictureItemNoBufferData pictureItemNoBufferData)
        {
            ID = pictureItemNoBufferData.ID;
            DisplayOrder = pictureItemNoBufferData.DisplayOrder;
            Artist = pictureItemNoBufferData.Artist;
            Price = pictureItemNoBufferData.Price;
            Media = pictureItemNoBufferData.Media;
            Theme = pictureItemNoBufferData.Theme;
            Size = pictureItemNoBufferData.Size;
            Created = pictureItemNoBufferData.Created;
            UserId = pictureItemNoBufferData.UserId;
        }

        public int ID { get; set; }
        [Display(Name = "Order")]
        public int? DisplayOrder { get; set; }
        public string Title { get; set; }        
        public decimal Price { get; set; }        
        public string Artist { get; set; }        
        public byte[] ImageT { get; set; }
        public string Media { get; set; }
        public string Theme { get; set; }
        public string Size { get; set; }
        public DateTime Created { get; set; }
        public long UserId { get; set; }    

        [NotMapped]        
        public Bitmap Image
        {
            
            get
            {
                Bitmap newBmp =null;
                if (ImageT != null)
                {
                    newBmp = ConvertToBitmap(ImageT);
                                                          
                }
                return newBmp;
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
    }
}