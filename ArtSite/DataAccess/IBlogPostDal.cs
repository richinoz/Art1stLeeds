using System.Data.Entity;
using ArtSite.DataAccess;
using MobileBlog.Models;

namespace ArtSite.Models
{
    public interface IBlogPostDal:IDAL<MobilePost>
    {

    }

    public class BlogPostDal : EntityFrameworkDal<MobilePost>, IBlogPostDal
    {
        public BlogPostDal(DbContext artGalleryDbContext) : base(artGalleryDbContext) { }
      

    }
}