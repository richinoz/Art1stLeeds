using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ArtSite.Filters;

namespace ArtSite.Models
{
    public class EMailView
    {
        [Required]
        [Display(Name = "Message details")]
        public string Message { get; set; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address from")]
        public string EmailFrom { get; set; }

      
        public LogOnModel LogOnModel { get; set; }
    }

    public class MenuRatingModel
    {
        [Display(Name = "Overall Rating")]
        [Required]
        public byte OverallRating { get; set; }
        public MenuRatingListEntryModel[] ListEntries { get; set; }
        public LogOnModel LogOn { get; set; }
    }

    public class MenuRatingListEntryModel
    {
        [Display(Name = "Menu Item")]
        [Required]
        public string MenuItemName { get; set; }

        [Display(Name = "Satisfactory?")]
        public bool WasSatisfactory { get; set; }

        [Display(Name = "If not satisfactory, explain why.")]
        public string Explanation { get; set; }
    }
}