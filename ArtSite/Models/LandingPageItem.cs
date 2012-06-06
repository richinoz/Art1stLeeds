using System.ComponentModel.DataAnnotations;

namespace ArtSite.Models
{
    public class LandingPageItem
    {
        [Key]
        public int Id { get; set; }

        public int PictureId { get; set; } 

    }
}