using NewsSite.Data.UnitOfWork;
using NewsSite.Service.CategoryServices;
using NewsSite.Service.PostServices;
using NewsSite.Web.Framework.Controllers;
using System;
using System.Linq;
using System.Web.Mvc;

namespace NewsSite.Web.Controllers
{
    public class PostController : PublicController
    {
        private readonly IPostService _postService;
        private readonly ICategoryService _categoryService;

        public PostController(IUnitOfWork uow, IPostService postService, ICategoryService categoryService)
            : base(uow)
        {
            _categoryService = categoryService;
            _postService = postService;
        }

        public ActionResult CategoryDetail(int id)
        {
            var category = _categoryService.Find(id);

            return View(category);
        }

        public ActionResult PostDetail(int id)
        {
            var post = _postService.Find(id);

            post.ReadCount++;
            _postService.Update(post);
            _uow.SaveChanges();

            return View(post);
        }
    }
}