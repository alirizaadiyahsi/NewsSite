using NewsSite.Core.Application;
using NewsSite.Data.UnitOfWork;
using System.Web.Mvc;

namespace NewsSite.Web.Framework.Controllers
{
    [Authorize(Roles = Keys.Admin)]
    public class AdminController : BaseController
    {
        public AdminController(IUnitOfWork uow)
            : base(uow)
        {
        }
    }
}
