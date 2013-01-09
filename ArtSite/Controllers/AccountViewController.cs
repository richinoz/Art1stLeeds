using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using ArtSite.Filters;
using ArtSite.Models;
using ArtSite.DataAccess;

namespace ArtSite.Controllers
{
    [Authorize]
    public class AccountViewController : Controller
    {
        private ArtGalleryDBContext db = new ArtGalleryDBContext();

        
        // GET: /AccountView/
        public ActionResult Index()
        {
            if(Permissions.IsAdmin())
                return View(db.Users.ToList());

            LogOnModel currentUser = Permissions.GetCurrentUser();
            var userList = UserDal.AllUsers.Where(x => x.UserId == currentUser.UserId);
            return View(userList.ToList());
        }

        private ActionResult RedirectToLogin()
        {
            RouteValueDictionary dictionary = null;
            if (Request.Url != null)
                dictionary = new RouteValueDictionary(new { returnUrl = Request.Url.PathAndQuery });

            return RedirectToAction("LogOn", "Account", dictionary);
            
        }

       
        //
        // GET: /AccountView/Details/5        
        public ViewResult Details(long id)
        {
            LogOnModel logonmodel = db.Users.Find(id);
            return View(logonmodel);
        }

        //
        // GET: /AccountView/Create
        [AdminPermissions]
        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /AccountView/Create        
        [HttpPost, AdminPermissions]
        public ActionResult Create(LogOnModel logonmodel)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(logonmodel);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(logonmodel);
        }
        
        //
        // GET: /AccountView/Edit/5       
        public ActionResult Edit(long id)
        {
            LogOnModel logonmodel = db.Users.Find(id);
            return View(logonmodel);
        }

        //
        // POST: /AccountView/Edit/5
    
        [HttpPost]
        public ActionResult Edit(LogOnModel logonmodel)
        {         
            if (ModelState.IsValid)
            {
                IUserDal userDal = new UserDal(db);
                userDal.Entry(logonmodel).State = EntityState.Modified;
                userDal.SaveChanges();

                var currentUser = Membership.GetUser(logonmodel.UserName);
                currentUser.Email = logonmodel.Email;
                Membership.UpdateUser(currentUser);

                return RedirectToAction("Index");
            }
            return View(logonmodel);
        }

        //
        // GET: /AccountView/Delete/5
        [AdminPermissions]
        public ActionResult Delete(long id)
        {
            LogOnModel logonmodel = db.Users.Find(id);
            return View(logonmodel);
        }

        //
        // POST: /AccountView/Delete/5

        [HttpPost, ActionName("Delete"), AdminPermissions]
        public ActionResult DeleteConfirmed(long id)
        {
         
            LogOnModel logonmodel = db.Users.Find(id);
            db.Users.Remove(logonmodel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        
    }
}