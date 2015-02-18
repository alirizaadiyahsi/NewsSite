using System.Web.Mvc;
using System.Web.Routing;

namespace NewsSite.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "PostDetail",
                url: "PostDetail/{category}/{id}/{slug}",
                defaults: new { controller = "Post", action = "PostDetail", category = UrlParameter.Optional, id = UrlParameter.Optional, slug = UrlParameter.Optional },
                namespaces: new[] { "NewsSite.Web.Controllers" }
            );

            routes.MapRoute(
                name: "CategoryDetail",
                url: "CategoryDetail/{id}/{slug}",
                defaults: new { controller = "Post", action = "CategoryDetail", id = UrlParameter.Optional, slug = UrlParameter.Optional },
                namespaces: new[] { "NewsSite.Web.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "NewsSite.Web.Controllers" }
            );
        }
    }
}
