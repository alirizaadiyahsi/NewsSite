using System.Collections.Generic;

namespace NewsSite.Core.Database.Tables
{
    public class PictureGalery : BaseEntity
    {
        public PictureGalery()
        {
            Pictures = new HashSet<Picture>();
        }

        public string Name { get; set; }
        public string Slug { get; set; }
        public int? PostId { get; set; }

        public virtual Post Post { get; set; }

        public virtual ICollection<Picture> Pictures { get; set; }
    }
}
