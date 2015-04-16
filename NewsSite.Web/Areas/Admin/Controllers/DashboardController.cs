using NewsSite.Data.UnitOfWork;
using NewsSite.Web.Framework.Controllers;
using System.Web.Mvc;

namespace NewsSite.Web.Areas.Admin.Controllers
{
    public class DashboardController : PublicController
    {
        public DashboardController(IUnitOfWork uow)
            : base(uow)
        { 
        
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}