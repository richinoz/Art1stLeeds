using System;

namespace ArtSite.Models
{
    public class PictureItemNoBufferData
    {
        public PictureItemNoBufferData()
        {
        }

        public PictureItemNoBufferData(PictureItem picture)
        {

            Artist = picture.Artist;
            Created = picture.Created;
            ID = picture.ID;
            Media = picture.Media;
            Price = picture.Price;
            Size = picture.Size;
            Theme = picture.Theme;
            Title = picture.Title;
            UserId = picture.UserId;
            DisplayOrder = picture.DisplayOrder;

        }

     

        public int ID { get; set; }
        public int? DisplayOrder { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Artist { get; set; }
        public string Media { get; set; }
        public string Theme { get; set; }
        public string Size { get; set; }
        public DateTime Created { get; set; }
        public long UserId{ get; set; }    

    }
}