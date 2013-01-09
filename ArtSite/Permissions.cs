using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using ArtSite.DataAccess;
using ArtSite.Filters;
using ArtSite.Models;

namespace ArtSite
{
    public interface IPermissions
    {
        string Values { get; }
    }

    public static class Permissions
    {
        public static bool IsAdmin()
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var loggedInUser = HttpContext.Current.User.Identity;
                var user = UserDal.AllUsers.FirstOrDefault(x => x.UserName.ToLower() == loggedInUser.Name.ToLower());

                if (user != null && user.Permissions != null && user.Permissions.Contains(new AdminPermissions().Values))
                    return true;
            }
            return false;   
        }


        public static bool HasPermission(string userName)
        {
            var loggedInUser = HttpContext.Current.User.Identity;
            if (loggedInUser.IsAuthenticated)
            {
                var user = UserDal.AllUsers.FirstOrDefault(x => x.UserName.ToLower() == loggedInUser.Name.ToLower());

                if (user != null && user.UserName.ToLower() == userName.ToLower())
                    return true;
            }
            return false;

           
        }

        public static bool HasPermission(IPrincipal user, IPermissions permissions)
        {
            ArtGalleryDBContext db = new ArtGalleryDBContext();
            IUserDal userDal = new UserDal(db);

            var currentUser = GetCurrentUser(userDal, user);
                
            if (currentUser != null && currentUser.Permissions!=null)
            {
                
                if (currentUser.Permissions.Contains(permissions.Values))
                    return true;
            }
            
            return false;
        }

        public static LogOnModel GetCurrentUser()
        {
            return GetCurrentUser(null, HttpContext.Current.User);
        }
        public static LogOnModel GetCurrentUser(IUserDal userDal, IPrincipal user)
        {
            if (user == null)
                return null;

           var ret = UserDal.AllUsers.FirstOrDefault(x => x.UserName.ToLower() == user.Identity.Name.ToLower());
            
           if(ret==null && userDal !=null) 
                ret = userDal.Enitities.FirstOrDefault(x => x.UserName.ToLower() == user.Identity.Name.ToLower());

            return ret;
        }

    }
}