using System.Collections.Generic;
namespace NewsSite.Core.Database.Tables
{
    public class Tag : BaseEntity
    {
        public Tag()
        {
            Posts = new HashSet<Post>();
        }

        public string Name { get; set; }
        public string Slug { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
