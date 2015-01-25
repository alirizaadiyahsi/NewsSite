using NewsSite.Web.Framework.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NewsSite.Web.Areas.Admin.Models
{
    public class CategoryModel : BaseViewModel
    {
        [DisplayName("Kategori")]
        public string Name { get; set; }

        [DisplayName("Sıra")]
        [Required(ErrorMessage="{0} alanı gereklidir!")]
        public int Order { get; set; }
    }
}