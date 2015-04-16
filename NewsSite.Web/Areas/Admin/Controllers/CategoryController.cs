using NewsSite.Core.Database.Tables;
using NewsSite.Data.UnitOfWork;
using NewsSite.Service.CategoryServices;
using NewsSite.Utilities;
using NewsSite.Web.Areas.Admin.Models;
using NewsSite.Web.Framework.Controllers;
using NewsSite.Web.Framework.Membership;
using System;
using System.Linq;
using System.Web.Mvc;

namespace NewsSite.Web.Areas.Admin.Controllers
{
    public class CategoryController : PublicController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(IUnitOfWork uow, ICategoryService categoryService)
            : base(uow)
        {
            _categoryService = categoryService;
        }

        public ActionResult Index()
        {
            var categories = _categoryService.GetAll();

            return View(categories.OrderBy(x => x.Id));
        }

        public ActionResult AddCategory()
        {
            var model = new CategoryModel();
            model.Order = 0;
            model.IsActive = true;

            return View(model);
        }

        [HttpPost]
        public ActionResult AddCategory(CategoryModel model)
        {
            var category = new Category();

            if (ModelState.IsValid)
            {
                category.Description = model.Description;
                category.InsertDate = DateTime.Now;
                category.InsertUserId = CustomMembership.CurrentUser().Id;
                category.IsActive = model.IsActive;
                category.Name = model.Name;
                category.Order = model.Order;
                category.Slug = StringManager.ToSlug(model.Name);

                try
                {
                    _categoryService.Insert(category);
                    _uow.SaveChanges();

                    messagesForView.Clear();
                    messagesForView.Add("İşlemi başarılı!");
                    Success(messagesForView);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    messagesForView.Clear();
                    messagesForView.Add("İşlem başarısız!");
                    messagesForView.Add(ex.Message);
                    messagesForView.Add(ex.InnerException.Message);
                    Error(messagesForView);
                }
            }

            return View(model);
        }

        public ActionResult EditCategory(int id)
        {
            var model = new CategoryModel();
            var category = _categoryService.Find(id);

            model.Description = category.Description;
            model.Id = category.Id;
            model.IsActive = category.IsActive;
            model.Name = category.Name;
            model.Order = category.Order;

            return View(model);
        }

        [HttpPost]
        public ActionResult EditCategory(CategoryModel model)
        {
            var category = _categoryService.Find(model.Id);

            if (ModelState.IsValid)
            {
                category.Description = model.Description;
                category.IsActive = model.IsActive;
                category.Name = model.Name;
                category.Order = model.Order;
                category.Slug = StringManager.ToSlug(model.Name);
                category.UpdateUserId = CustomMembership.CurrentUser().Id;
                category.UpdateDate = DateTime.Now;

                try
                {
                    _categoryService.Update(category);
                    _uow.SaveChanges();

                    messagesForView.Clear();
                    messagesForView.Add("İşlemi başarılı!");
                    Success(messagesForView);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    messagesForView.Clear();
                    messagesForView.Add("İşlem başarısız!");
                    messagesForView.Add(ex.Message);
                    messagesForView.Add(ex.InnerException.Message);
                    Error(messagesForView);
                }
            }

            return View(model);
        }

        public ActionResult DeleteCategory(int id)
        {
            var category = _categoryService.Find(id);
            try
            {
                _categoryService.Delete(category);
                _uow.SaveChanges();

                messagesForView.Clear();
                messagesForView.Add("İşlemi başarılı!");
                Success(messagesForView);
            }
            catch (Exception ex)
            {
                messagesForView.Clear();
                messagesForView.Add("İşlem başarısız!");
                messagesForView.Add(ex.Message);
                messagesForView.Add(ex.InnerException.Message);
                Error(messagesForView);
            }

            return RedirectToAction("Index");
        }
    }
}