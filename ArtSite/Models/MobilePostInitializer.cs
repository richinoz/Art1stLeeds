using System;
using System.Collections.Generic;
using System.Data.Entity;
using ArtSite.DataAccess;
using ArtSite.Models;

namespace MobileBlog.Models
{
    public class MobilePostInitializer : DropCreateDatabaseIfModelChanges<ArtGalleryDBContext>
    {
        protected override void Seed(ArtGalleryDBContext context)
        {
            var posts = new List<MobilePost>
                            {

                                new MobilePost
                                    {
                                        Title = "My new blog",
                                        Content = "This is my first blog post...",
                                        PublishDate = DateTime.Now
                                    },

                                new MobilePost
                                    {
                                        Title = "Second post",
                                        Content = "This is my second blog post. Hi!",
                                        PublishDate = DateTime.Now
                                    }
                            };

            IBlogPostDal blogPostDal = new BlogPostDal(context);
            posts.ForEach(d => blogPostDal.Enitities.Add(d));
        }
    }
}
