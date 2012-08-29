using System.Data.Entity.ModelConfiguration;
using ArtSite.Models;

namespace ArtSite.DataAccess
{
    public class LogOnModelEntityTypeConfiguration : EntityTypeConfiguration<LogOnModel>
    {
        public LogOnModelEntityTypeConfiguration()
        {
            HasKey(x => x.UserId);
            //Property(x => x.Id).HasColumnName("ID").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.UserName);

        }
    }
}