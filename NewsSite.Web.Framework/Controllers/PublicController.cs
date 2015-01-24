using NewsSite.Data.UnitOfWork;

namespace NewsSite.Web.Framework.Controllers
{
    public class PublicController : BaseController
    {
        public PublicController(IUnitOfWork uow)
            : base(uow)
        {
        }
    }
}
