using NewsSite.Web.Framework.Models;
using System.ComponentModel;

namespace NewsSite.Web.Areas.Admin.Models
{
    public class TagModel:BaseViewModel
    {
        [DisplayName("Tag")]
        public string Name { get; set; }
    }
}