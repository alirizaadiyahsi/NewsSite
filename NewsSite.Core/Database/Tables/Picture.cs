using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSite.Core.Database.Tables
{
    public class Picture : BaseEntity
    {
        public string ImgUrl { get; set; }
        public string ImgUrlSmall { get; set; }
        public string ImgUrlMiddle { get; set; }
        public string ImgUrlBig { get; set; }
        public int Order { get; set; }
        public int ContentSize { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public int PictureGaleryId { get; set; }

        public virtual PictureGalery PictureGalery { get; set; }
    }
}
