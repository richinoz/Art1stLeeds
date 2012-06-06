using System.Data.Entity;
using ArtSite.Models;

namespace ArtSite.DataAccess
{
    public interface IPictureDal:IDAL<PictureItem>
    {

    }

    public class PictureDal : EntityFrameworkDal<PictureItem>, IPictureDal
    {
        public PictureDal(DbContext artGalleryDbContext):base(artGalleryDbContext){}
       
    }

    public interface ILandingPageDal : IDAL<LandingPageItem>
    {

    }

    public class LandingPageDal : EntityFrameworkDal<LandingPageItem>, ILandingPageDal
    {
        public LandingPageDal(DbContext artGalleryDbContext) : base(artGalleryDbContext) { }

       
    }
}