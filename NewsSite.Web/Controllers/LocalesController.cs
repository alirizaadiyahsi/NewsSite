using System;
using System.Web.Mvc;
using Insya.Localization;

namespace NewsSite.Web.Controllers
{
    public class LocalesController : Controller
    {

        public ActionResult Index(string lang = "en_US")
        {
            Response.Cookies["CacheLang"].Value = lang;
			
            if (Request.UrlReferrer != null)
                Response.Redirect(Request.UrlReferrer.ToString());
             
			var message = Localization.Get("changedlng");
    
			return Content(message);
        }

    }
}