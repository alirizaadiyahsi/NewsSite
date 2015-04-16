using NewsSite.Core.Database.Tables;
using NewsSite.Web.Framework.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace NewsSite.Web.Areas.Admin.Models
{
    public class PictureGaleryModel : BaseViewModel
    {
        [DisplayName("Galeri")]
        public string Name { get; set; }
    }

    public class PictureModel : BaseViewModel
    {
        [Required(ErrorMessage = "{0} alanı gereklidir!")]
        public HttpPostedFileBase PictureGaleryImg { get; set; }

        [DisplayName("Sıra")]
        public int Order { get; set; }

        public PictureGalery PictureGalery { get; set; }
    }
}