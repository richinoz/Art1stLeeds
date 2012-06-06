using System.Collections.Generic;

namespace ArtSite.Models.Views
{
    public class LandingPageChooserViewModel
    {
        public List<PictureItemNoBufferData> Pictures { get; set; }
        public int LandingPageItemId { get; set; }
    }

   public class LandingPageItemViewModel
   {
       public PictureItemNoBufferData Picture { get; set; }
       public int LandingPageItemId { get; set; }
   }
}