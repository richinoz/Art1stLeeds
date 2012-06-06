using System.Data.Entity;
using ArtSite.DataAccess;
using MobileBlog.Models;

namespace ArtSite.Models
{
    public interface IUserBlogDal:IDAL<UserBlog>
    {

    }

    public class UserBlogDal : EntityFrameworkDal<UserBlog>, IUserBlogDal
    {
        public UserBlogDal(DbContext artGalleryDbContext) : base(artGalleryDbContext) { }
      

    }
}