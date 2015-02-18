using System.Web.Mvc;

namespace NewsSite.Web.Views
{
    public class PostController : Controller
    {
        public ActionResult Detail(int postId)
        {
            return View();
        }

        public ActionResult Detail()
        {
            return View();
        }
    }
}