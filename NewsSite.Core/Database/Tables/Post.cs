using System.Collections.Generic;
namespace NewsSite.Core.Database.Tables
{
    public class Post : BaseEntity
    {
        public Post()
        {
            Galeries = new HashSet<Galery>();
            Tags = new HashSet<Tag>();
        }

        public string Title { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }
        public string TagNames { get; set; }
        public string ImgUrl { get; set; }
        public string ImgUrlSmall { get; set; }
        public string ImgUrlMiddle { get; set; }
        public string ImgUrlBig { get; set; }
        public int? AuthorId { get; set; }
        public int ReadCount { get; set; }
        public int CommentCount { get; set; }
        public string Source { get; set; }
        public int CategoryId { get; set; }
        public int PostPositionId { get; set; }

        public virtual Category Category { get; set; }
        public virtual PostPosition PostPosition { get; set; }

        public virtual ICollection<Galery> Galeries { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
    }
}
