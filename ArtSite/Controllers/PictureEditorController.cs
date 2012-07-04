using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ArtSite.DataAccess;
using ArtSite.Extensions;
using ArtSite.Models;
using ArtSite.Models.Views;

namespace ArtSite.Controllers
{
    [Authorize]
    public class PictureEditorController : Controller
    {
        private ArtGalleryDBContext _db;
        private IPictureDal _pictureDal;
        private IUserDal _userDal;

        public PictureEditorController() : this(null, null) { }

        public PictureEditorController(IPictureDal pictureDal, IUserDal userDal)
        {
            _db = new ArtGalleryDBContext();

            if(pictureDal==null)
                pictureDal = new PictureDal(_db);

            if (userDal == null)
                userDal = new UserDal(_db);

            _pictureDal = pictureDal;
            _userDal = userDal;

        }
        //
        // GET: /PictureLoaderDB/

        public ActionResult Index(string name)
        {
            if (name != null)
            {
                if (name == "blanksearch")
                    name = "";
                ViewBag.SearchCriteria = name;
                return View(GetImagesFromDb(name));
            }
            return View();
        }

        public ActionResult Searching(string name)
        {
            string tempname = (string)TempData["PictureDBSearch"];
            if(name!=null)
                TempData["PictureDBSearch"] = name;
            else            
                name = tempname;

            if (Request.IsAjaxRequest())
                return PartialView(GetImagesFromDb(name));

            if (name == "")
                name = "blanksearch";

            return RedirectToAction("Index", new {name});
          
        }
        //
        // GET: /PictureLoaderDB/Details/5

        public ViewResult Details(int id)
        {
            PictureItem pictureitem = _pictureDal.Enitities.Find(id);
            return View(pictureitem);
        }

        //
        // GET: /PictureLoaderDB/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /PictureLoaderDB/Create

        [HttpPost]
        public ActionResult Create(PictureItem pictureitem)
        {
            if (ModelState.IsValid)
            {
                _pictureDal.Enitities.Add(pictureitem);
                _pictureDal.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(pictureitem);
        }
        
        //
        // GET: /PictureLoaderDB/Edit/5
    
        public ActionResult Edit(int id, string returnUrl)
        {
            if (!Request.IsAuthenticated)
            {

                RouteValueDictionary dictionary = null;
                if (Request.Url != null)
                    dictionary = new RouteValueDictionary(new { returnUrl = Request.Url.PathAndQuery });

                return RedirectToAction("LogOn", "Account", dictionary);
            }

            var pictureitem = _pictureDal.Enitities.Find(id);

            var editPictureViewModel = new EditPictureViewModel()
                                                            {
                                                                Picture = pictureitem,
                                                                ReturnUrl = returnUrl
                                                            };
            return View(editPictureViewModel);
        }

        //
        // POST: /PictureLoaderDB/Edit/5

        [HttpPost]
        public ActionResult Edit(EditPictureViewModel editPictureTtem)  
        {
            var pictureitem = editPictureTtem.Picture;
            if (ModelState.IsValid)
            {
                var pic = pictureitem;
               
                _pictureDal.Enitities.Attach(pic);

                _pictureDal.Entry(pic).Property(x => x.Artist).IsModified = true;
                _pictureDal.Entry(pic).Property(x => x.Title).IsModified = true;
                _pictureDal.Entry(pic).Property(x => x.Price).IsModified = true;
                _pictureDal.Entry(pic).Property(x => x.Media).IsModified = true;
                _pictureDal.Entry(pic).Property(x => x.Size).IsModified = true;
                _pictureDal.Entry(pic).Property(x => x.Theme).IsModified = true;
                _pictureDal.Entry(pic).Property(x => x.DisplayOrder).IsModified = true;

                _pictureDal.SaveChanges();

                if (editPictureTtem.ReturnUrl != null)
                    return Redirect(editPictureTtem.ReturnUrl);

                return RedirectToAction("Searching");
            }
            return View(pictureitem);
        }

    
        //
        // GET: /PictureLoaderDB/Delete/5
 
        public ActionResult Delete(int id, string returnUrl)
        {
            PictureItem pictureitem = _pictureDal.Enitities.Find(id);
            var editPictureViewModel = new EditPictureViewModel
            {
                Picture = pictureitem,
                ReturnUrl = returnUrl
            };

            return View(editPictureViewModel);
        }
   

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(EditPictureViewModel editPictureViewModel)
        {
            int id = editPictureViewModel.Picture.ID;
            if (User.Identity.IsAuthenticated)
            {
                var hasPermission =  Permissions.HasPermission(User.Identity.Name);
                LogOnModel currentUser = Permissions.GetCurrentUser(_userDal, User);// UserDal.AllUsers.FirstOrDefault(x => x.UserName == User.Identity.Name) ?? _userDal.Enitities.FirstOrDefault(x => x.UserName == User.Identity.Name);

                PictureItem pictureitem = _pictureDal.Enitities.Find(id);

                if (currentUser != null && pictureitem != null)
                    hasPermission |= currentUser.UserName == pictureitem.Artist;

                if (hasPermission)
                {
                    _pictureDal.Enitities.Remove(pictureitem);
                    _db.SaveChanges();

                    if (editPictureViewModel.ReturnUrl!=null)
                        return Redirect(editPictureViewModel.ReturnUrl); 

                    return RedirectToAction("Searching");
                }
            }

            ModelState.AddModelError("", "You do not have permission to delete");
            return View();
        }

        public static List<PictureItemNoBufferData> GetImagesFromDbMin(LogOnModel user)
        {
            return GetImagesFromDbMin(user, null);
        }

        public static List<PictureItemNoBufferData> GetImagesFromDbMin(LogOnModel user, string theme)
        {
            using (var db = new ArtGalleryDBContext())
            {
                var userId = user != null ? user.UserId : -1;

                var pictureDal = new PictureDal(db);

                var model = pictureDal.Enitities.Where(x => x.UserId == userId);

                if (theme != null)
                    model = theme==string.Empty ? model.Where(x => x.Theme.IsNullOrWhiteSpace()) : model.Where(x => x.Theme.ToLower().Contains(theme.ToLower()));

                var query = from c in model
                            select new PictureItemNoBufferData()
                            {
                                Artist = c.Artist,
                                Created = c.Created,
                                ID = c.ID,
                                Media = c.Media,
                                Price = c.Price,
                                Size = c.Size,
                                Theme = c.Theme,
                                Title = c.Title,
                                UserId = c.UserId,
                                DisplayOrder = c.DisplayOrder

                            };

                return query.OrderBy(x => x.DisplayOrder ?? 9999).ToList();
            }
        }

        public static List<PictureItem> GetImagesFromDb(string searchName)
        {
            using (ArtGalleryDBContext db = new ArtGalleryDBContext())
            {
                IEnumerable<PictureItemNoBufferData> query = null;

                var x1 = GetImagesQuery(db, searchName);

                return x1.ToList();
            }

        }

        //PictureItemNoBufferData
        public static List<LandingPageItemViewModel> GetImagesForLandingPage()
        {
            using (var db = new ArtGalleryDBContext())
            {
                var pictureDal = new PictureDal(db);

                var landingPageDal = new LandingPageDal(db);
                var fullList = landingPageDal.Enitities.ToList();
                var list = fullList.Select(x => x.PictureId).ToList();


                var selected =
                    from p in pictureDal.Enitities
                    from l in list
                    where p.ID == l
                    select new PictureItemNoBufferData()

                               {
                                   Artist = p.Artist,
                                   Created = p.Created,
                                   ID = p.ID,
                                   Media = p.Media,
                                   Price = p.Price,
                                   Size = p.Size,
                                   Theme = p.Theme,
                                   Title = p.Title,
                                   UserId = p.UserId,
                                   DisplayOrder = p.DisplayOrder 
                               };

                var ret = selected.ToList();

                List<LandingPageItemViewModel>  list2 = new List<LandingPageItemViewModel>();
                foreach (var landingPageItem in fullList)
                {
                    var pictureItemNoBufferData = ret.FirstOrDefault(x=>x.ID == landingPageItem.PictureId);
                    if (pictureItemNoBufferData==null)
                    {
                        //LogOnModel logOnModel = Permissions.GetCurrentUser();
                        //if(logOnModel!=null)
                        {
                            pictureItemNoBufferData = new PictureItemNoBufferData()
                                                          {UserId = 1};
                        }
                      
                    }
                    if (pictureItemNoBufferData!=null)
                    {
                        var landingPageItemViewModel = new LandingPageItemViewModel()
                                                           {
                                                               LandingPageItemId = landingPageItem.Id,
                                                               Picture = pictureItemNoBufferData
                                                           };


                        list2.Add(landingPageItemViewModel);
                    }

                }

                return list2;

            }

        }
        public static List<PictureItemNoBufferData> GetImagesFromDbMin(string searchName)
        {
            using (ArtGalleryDBContext db = new ArtGalleryDBContext())
            {
                IQueryable<PictureItemNoBufferData> query = null;

                var x1 = GetImagesQuery(db, searchName);

                query = from c in x1
                        select new PictureItemNoBufferData()
                        {
                            Artist = c.Artist,
                            Created = c.Created,
                            ID = c.ID,
                            Media = c.Media,
                            Price = c.Price,
                            Size = c.Size,
                            Theme = c.Theme,
                            Title = c.Title,
                            UserId = c.UserId,
                            DisplayOrder = c.DisplayOrder 

                        };


                return query.OrderBy(x=>x.DisplayOrder ?? 9999).ToList();

            }

        }

       


        private static IQueryable<PictureItem> GetImagesQuery(ArtGalleryDBContext db, string searchName) 
        {
           
                var pictureDal = new PictureDal(db);

                bool except = false;

                if (String.IsNullOrEmpty(searchName))
                    searchName = string.Empty;

                if (searchName.Length > 0 && searchName.Substring(0, 1) == "!")
                {
                    searchName = searchName.Substring(1);
                    except = true;
                }

                IQueryable<PictureItem> query = null;

                if (searchName.ToLower() == "anon")
                    query =
                        pictureDal.Enitities.Where(x => (x.Artist == null) || (x.Artist.ToLower() == "anon"));

                if (searchName.ToLower() == "untitled")
                    query =
                        pictureDal.Enitities.Where(x => (x.Title == null) || (x.Title.ToLower() == "untitled"));

                if (query == null)
                    if (searchName != string.Empty)
                    {
                        searchName = searchName.ToLower();

                        query = pictureDal.Enitities.Where(x => (x.Title != null && x.Title.ToLower().Contains(searchName)) ||
                                                       (x.Artist != null && x.Artist.ToLower().Contains(searchName)) ||
                                                       (x.Media != null && x.Media.ToLower().Contains(searchName)) ||
                                                       (x.Theme != null && x.Theme.ToLower().Contains(searchName)));
                    }
                    else
                        query = pictureDal.Enitities;


                if (except)
                    return pictureDal.Enitities.Except(query);

            return query;

        }


        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}