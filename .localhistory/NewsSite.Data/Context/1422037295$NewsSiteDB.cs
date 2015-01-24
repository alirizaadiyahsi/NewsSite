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

        // veritabanı tablolarını temsil edecek sınıflar
        // örneğin:
        // public virtual DbSet<User> User { get; set; }
        // public virtual DbSet<Role> Role { get; set; }

        /// <summary>
        /// Tablolar oluşturulurken yapmak istediğimiz ayarları yapar.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Tablolar oluşturulurken tablo isimlerinin 
            // sonuna 's' karakteri eklemesini istemiyoruz
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        /// <summary>
        /// Veritabanı oluşturma stratejisini belirler.
        /// </summary>
        public class SeedBestContext : CreateDatabaseIfNotExists<NewsSiteDB>
        {
            protected override void Seed(NewsSiteDB context)
            {
                base.Seed(context);
            }
        }
    }
}
