using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace ArtSite.DataAccess
{
    public interface IDAL<T> where T : class
    {
        DbEntityEntry<T> Entry(T entity);
        IDbSet<T> Enitities { get; }
        void SaveChanges();
    }

    public class EntityFrameworkDal<T> : IDAL<T> where T : class
    {
        private DbContext _artGalleryDbContext;

        public EntityFrameworkDal(DbContext artGalleryDbContext)
        {
            _artGalleryDbContext = artGalleryDbContext;
            
        }

        public virtual IDbSet<T> Enitities
        {
            get { return _artGalleryDbContext.Set<T>(); }
        }


        public DbEntityEntry<T> Entry(T entity)
        {
            return _artGalleryDbContext.Entry(entity);
        }


        public void SaveChanges()
        {
            _artGalleryDbContext.SaveChanges();
        }

    }
}