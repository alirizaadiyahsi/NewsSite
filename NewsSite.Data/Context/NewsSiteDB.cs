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
        public virtual DbSet<PictureGalery> PictureGalery { get; set; }
        public virtual DbSet<Picture> Picture { get; set; }
        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<PostPosition> PostPosition { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
