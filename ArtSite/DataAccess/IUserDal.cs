using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ArtSite.DataAccess;

namespace ArtSite.Models
{
    public interface IUserDal:IDAL<LogOnModel>
    {

    }

    public class UserDal : EntityFrameworkDal<LogOnModel>, IUserDal
    {
        public UserDal(DbContext artGalleryDbContext) : base(artGalleryDbContext) { }

        private static List<LogOnModel> _allUsers = null;

        public static List<LogOnModel> AllUsers
        {
            get
            {
                if (_allUsers  == null)
                {
                    GetAllUsers();
                }
                return _allUsers;
            }
        }

        private static void GetAllUsers()
        {
            using (var db = new ArtGalleryDBContext())
            {
                IUserDal userDal = new UserDal(db);
                _allUsers = userDal.Enitities.OrderBy(x => x.UserName).ToList();
            }
        }

        public new void SaveChanges()
        {
            base.SaveChanges();
            GetAllUsers();

        }
    }
}