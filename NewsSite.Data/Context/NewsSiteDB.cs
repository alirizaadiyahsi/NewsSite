using NewsSite.Core.Database.Tables;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace NewsSite.Data.Context
{
    public class NewsSiteDB : DbContext
    {
        public NewsSiteDB()
            : base("NewsSiteDB")
        {
            Configuration.LazyLoadingEnabled = true;
        }

        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Tag> Tag { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
