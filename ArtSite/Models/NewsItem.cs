using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ArtSite.Models
{
    public class NewsItem
    {
        [Key]
        public int Id { get; set; }

        [Required]        
        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Required]
        [Display(Name = "Body")]
        public string Body { get; set; }


    }
}