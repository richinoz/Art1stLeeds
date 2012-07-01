using System.Collections.Generic;

namespace ArtSite.Models.Views
{
    public class ArtistGalleryViewModel
    {
        public List<PictureItemNoBufferData> Pictures { get; set; }
        public Dictionary<string, List<PictureItemNoBufferData>> Categories { get; set; }
        public LogOnModel Artist { get; set; }
    }
}