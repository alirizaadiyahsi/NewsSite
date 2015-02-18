using NewsSite.Core.Application;
using NewsSite.Core.Database.Tables;
using NewsSite.Data.UnitOfWork;
using NewsSite.Service.CategoryServices;
using NewsSite.Service.PostServices;
using NewsSite.Web.Framework.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsSite.Web.Controllers
{
    public class HomeController : PublicController
    {
        private readonly IPostService _postService;
        private readonly ICategoryService _categoryService;

        public HomeController(IUnitOfWork uow, IPostService postService, ICategoryService categoryService)
            : base(uow)
        {
            _postService = postService;
            _categoryService = categoryService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _CategoryPosts()
        {
            var categories = _categoryService.GetAll()
                .OrderBy(x => x.Order);

            return PartialView(categories);
        }

        public ActionResult _SliderPosts()
        {
            var posts = _postService.GetAll()
                .OrderByDescending(x => x.Id)
                .Take(7)
                .ToList();

            return PartialView(posts);
        }

        public ActionResult _MostReaded()
        {
            var posts = _postService.GetAll()
                .OrderByDescending(x => x.ReadCount)
                .Take(5);

            return PartialView(posts);
        }

        public ActionResult _CategoriesMenu()
        {
            var categories = new List<Category>();

            if (Session[Keys.Categories] == null)
            {
                categories = _categoryService.GetAll().ToList();
                Session[Keys.Categories] = categories;
            }

            return PartialView();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}