using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ArtSite.DataAccess;
using ArtSite.Models;

namespace ArtSite.Helpers
{
    public class Helpers
    {
        public static LogOnModel GetUserForName(string artistName, DbContext db)
        {
            return UserDal.AllUsers.FirstOrDefault(x => x.UserName.Contains(artistName));
        }

        public static LogOnModel GetUserForId(long artistId, DbContext db)
        {
            return UserDal.AllUsers.SingleOrDefault(x => x.UserId.Equals(artistId));
        }
    }
}