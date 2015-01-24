using NewsSite.Data.UnitOfWork;
using System.Web.Mvc;

namespace NewsSite.Web.Framework.Controllers
{
    [Authorize]
    public class AuthorizedController : BaseController
    {
        public AuthorizedController(IUnitOfWork uow)
            : base(uow)
        {
        }
    }
}
