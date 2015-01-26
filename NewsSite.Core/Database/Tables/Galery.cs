using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public virtual ICollection<GaleryImage> GaleryImages { get; set; }
    }
}
