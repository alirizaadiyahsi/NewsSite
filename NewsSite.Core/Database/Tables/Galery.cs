using System.Collections.Generic;

namespace NewsSite.Core.Database.Tables
{
    public class Galery : BaseEntity
    {
        public Galery()
        {
            GaleryImages = new HashSet<GaleryImage>();
        }

        public string Name { get; set; }
        public string Slug { get; set; }
        public int? PostId { get; set; }

        public virtual Post Post { get; set; }

        public virtual ICollection<GaleryImage> GaleryImages { get; set; }
    }
}
