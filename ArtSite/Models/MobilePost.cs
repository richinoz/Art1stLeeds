using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace MobileBlog.Models
{
    public class MobilePost
    {
        public int? UserId { get; set; }
        public long ID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        public DateTime PublishDate { get; set; }
    }

  
}