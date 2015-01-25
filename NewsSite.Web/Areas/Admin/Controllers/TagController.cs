using NewsSite.Core.Database.Tables;
using NewsSite.Data.UnitOfWork;
using NewsSite.Service.TagServices;
using NewsSite.Utilities;
using NewsSite.Web.Areas.Admin.Models;
using NewsSite.Web.Framework.Controllers;
using NewsSite.Web.Framework.Membership;
using System;
using System.Web.Mvc;
using System.Linq;

namespace NewsSite.Web.Areas.Admin.Controllers
{
    public class TagController : AdminController
    {
        private readonly ITagService _tagService;

        public TagController(IUnitOfWork uow, ITagService tagService)
            : base(uow)
        {
            _tagService = tagService;
        }

        public ActionResult Index()
        {
            var tags = _tagService.GetAll();

            return View(tags.OrderBy(x => x.Id));
        }

        public ActionResult AddTag()
        {
            var model = new TagModel();
            model.IsActive = true;

            return View(model);
        }

        [HttpPost]
        public ActionResult AddTag(TagModel model)
        {
            var tag = new Tag();

            if (ModelState.IsValid)
            {
                tag.Description = model.Description;
                tag.InsertDate = DateTime.Now;
                tag.InsertUserId = CustomMembership.CurrentUser().Id;
                tag.IsActive = model.IsActive;
                tag.Name = model.Name;

                try
                {
                    _tagService.Insert(tag);
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

        public ActionResult EditTag(int id)
        {
            var model = new TagModel();
            var tag = _tagService.Find(id);

            model.Description = tag.Description;
            model.Id = tag.Id;
            model.IsActive = tag.IsActive;
            model.Name = tag.Name;

            return View(model);
        }

        [HttpPost]
        public ActionResult EditTag(TagModel model)
        {
            var tag = _tagService.Find(model.Id);

            if (ModelState.IsValid)
            {
                tag.Description = model.Description;
                tag.IsActive = model.IsActive;
                tag.Name = model.Name;
                tag.Slug = StringManager.ToSlug(model.Name);
                tag.UpdateUserId = CustomMembership.CurrentUser().Id;
                tag.UpdateDate = DateTime.Now;

                try
                {
                    _tagService.Update(tag);
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
    }
}