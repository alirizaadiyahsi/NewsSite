using System.Collections.Generic;
namespace NewsSite.Core.Database.Tables
{
    public class Category : BaseEntity
    {
        public Category()
        {
            Posts = new HashSet<Post>();
        }

        public string Name { get; set; }
        public string Slug { get; set; }
        public int Order { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
