using NewsSite.Core.Database.Tables;
using NewsSite.Web.Framework.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace NewsSite.Web.Areas.Admin.Models
{
    public class PostModel : BaseViewModel
    {
        [DisplayName("Başlık")]
        [Required(ErrorMessage = "{0} alanı gereklidir!")]
        public string Title { get; set; }

        [DisplayName("İçerik")]
        public string Content { get; set; }

        [DisplayName("Etiketler")]
        public string TagNames { get; set; }

        [DisplayName("Resim")]
        [Required(ErrorMessage = "{0} alanı gereklidir!")]
        public HttpPostedFileBase Img { get; set; }

        [DisplayName("Yazar")]
        public int? AuthorId { get; set; }

        [DisplayName("Kaynak")]
        public string Source { get; set; }

        [DisplayName("Kategori")]
        [Required(ErrorMessage = "{0} alanı gereklidir!")]
        public int CategoryId { get; set; }

        [DisplayName("Pozisyon")]
        [Required(ErrorMessage = "{0} alanı gereklidir!")]
        public int PositionId { get; set; }

        public List<string> ListTagNames { get; set; }

        [DisplayName("Galeriler")]
        public List<int> SelectedGaleryIds { get; set; }

        public IEnumerable<PostPosition> Positions { get; set; }
        public IEnumerable<Galery> Galeries { get; set; }
        public IEnumerable<User> Authors { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public string ImgUrl { get; set; }
    }
}