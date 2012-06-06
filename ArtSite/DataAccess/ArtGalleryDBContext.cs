using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using ArtSite.DataAccess;
using ArtSite.Models;
using MobileBlog.Models;

namespace ArtSite.DataAccess
{
   
    public class ArtGalleryDBContext : DbContext
    {       

        public ArtGalleryDBContext()
        {
            //var objectContext = (this as IObjectContextAdapter).ObjectContext;

            //// Sets the command timeout for all the commands
            //objectContext.CommandTimeout = 120;
        }
        public DbSet<PictureItem> Pictures { get; set; }
        public DbSet<LogOnModel> Users { get; set; }
        public DbSet<MobilePost> Posts { get; set; }
        public DbSet<LandingPageItem> LandingPage{ get; set; }
        public DbSet<NewsItem> NewsItem { get; set; }

        //public IQueryable<PictureItem> PictureItems
        //{
        //    get { return Pictures; }
        //}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {                       
            modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
            base.OnModelCreating(modelBuilder);                        
        }
       
     
    }

}