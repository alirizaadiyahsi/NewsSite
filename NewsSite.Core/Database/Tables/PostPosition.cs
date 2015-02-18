using System.Collections.Generic;

namespace NewsSite.Core.Database.Tables
{
    public class PostPosition : BaseEntity
    {
        public PostPosition()
        {
            Posts = new HashSet<Post>();
        }

        public string Name { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
