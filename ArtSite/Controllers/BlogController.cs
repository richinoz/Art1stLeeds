using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using ArtSite.DataAccess;
using MobileBlog.Models;
using ArtSite.Models;

namespace ArtSite.Controllers
{
    [Authorize]
    public class BlogController  : Controller
    {
        ArtGalleryDBContext db = new ArtGalleryDBContext();
        private IBlogPostDal _blogPostDal;        

        public BlogController()
        {
            _blogPostDal = new BlogPostDal(db);            
        }
        public ActionResult Index()
        {
            ViewBag.HeaderString = "Welcome!";

            var posts = from p in _blogPostDal.Enitities
                        where p.UserId == null
                        orderby p.ID descending 
                        select p;

            return View(posts.ToList());
        }

        public ActionResult Create()
        {
            var mobilePost = new MobilePost();
            ViewBag.HeaderString = "Create New Post";
            return View(mobilePost);
        }

        [Authorize]
        public ActionResult Edit(int blogId)
        {
          
            var mobilePost = _blogPostDal.Enitities.Find(blogId);
            if (mobilePost != null)
                return View("Create", mobilePost);                                
            
            return View("Create", new MobilePost());
        }

        [Authorize]
        public ActionResult CreateUserBlog()
        {
            var mobilePost = new MobilePost();

            var user = Helpers.Helpers.GetUserForName(User.Identity.Name, db);
            if (user!=null)
            {
                mobilePost.UserId = (int)user.UserId;
            }
            ViewBag.HeaderString = "Create New Post";

            return View("Create", mobilePost);
        }
 
             
        [HttpPost]
        public ActionResult SavePost(MobilePost post)
        {

            ViewBag.Message = "Failed: Title and Content cannot be empty...";

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(post.Title) & !string.IsNullOrEmpty(post.Content))
                {
                    if (post.ID == 0)
                    {
                        post.PublishDate = DateTime.Now;
                        _blogPostDal.Enitities.Add(post);
                    }
                    else
                    {
                        _blogPostDal.Enitities.Attach(post);
                        _blogPostDal.Entry(post).State = EntityState.Modified;                      
                    }
                    
                    db.SaveChanges();

                    //send email to administrator
                    SendEmailToAdministrator(post.Title, post.Content);

                    if(post.UserId>0)
                    {
                        return RedirectToAction("ArtistHome", "Gallery", new {userId = post.UserId});
                    }
                    return RedirectToAction("Index", "Blog");
                    
                    ViewBag.Message = "Message posted successfully!";                    
                }
            }


            if (Request.IsAjaxRequest())
            {                
                return PartialView();
            }

            return View("Create", post);

           // return "FAILED: Title and Comment must not be blank!";
        }

        private void SendEmailToAdministrator(string title, string message)
        {
            EMail.SendEmailToAdministrator(title, "from blog: " + message);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
