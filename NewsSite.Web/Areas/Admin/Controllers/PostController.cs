using NewsSite.Data.UnitOfWork;
using NewsSite.Service.PostServices;
using NewsSite.Utilities;
using NewsSite.Web.Framework.Controllers;
using NewsSite.Web.Framework.Membership;
using System;
using System.Web.Mvc;
using System.Linq;
using NewsSite.Web.Areas.Admin.Models;
using NewsSite.Core.Database.Tables;
using NewsSite.Service.CategoryServices;
using NewsSite.Service.TagServices;
using NewsSite.Service.PictureGaleryServices;
using System.IO;
using System.Drawing;
using NewsSite.Service.MembershipServices;

namespace NewsSite.Web.Areas.Admin.Controllers
{
    public class PostController : PublicController
    {
        private readonly IPostService _postService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;
        private readonly IPictureGaleryService _PictureGaleryService;
        private readonly IMembershipService _membershipService;

        public PostController(IUnitOfWork uow, IPostService postService, ICategoryService categoryService, ITagService tagService, IPictureGaleryService PictureGaleryService, IMembershipService membershipService)
            : base(uow)
        {
            _postService = postService;
            _categoryService = categoryService;
            _tagService = tagService;
            _PictureGaleryService = PictureGaleryService;
            _membershipService = membershipService;
        }

        public ActionResult Index()
        {
            var posts = _postService.GetAll();

            return View(posts.OrderBy(x => x.Id));
        }

        public ActionResult AddPost()
        {
            var model = new PostModel();
            model.IsActive = true;
            model.Categories = _categoryService.GetAll();
            model.ListTagNames = _tagService.GetAll().Select(x => x.Name).ToList();
            model.Positions = _postService.GetAllPositions();
            model.Galeries = _PictureGaleryService.GetAll();
            model.Authors = _membershipService.GetAllUsers();

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddPost(PostModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Img.ContentLength > 0)
                {
                    var image = model.Img;
                    var fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(image.FileName);
                    var imageDirectory = Server.MapPath("~/Content/Images/uploads/Post");
                    var imageDirectorySmall = Server.MapPath("~/Content/Images/uploads/Post/Small");
                    var imageDirectoryMiddle = Server.MapPath("~/Content/Images/uploads/Post/Middle");
                    var imageDirectoryBig = Server.MapPath("~/Content/Images/uploads/Post/Big");

                    // dizin yoksa oluştur.
                    if (!Directory.Exists(imageDirectory))
                    {
                        Directory.CreateDirectory(imageDirectory);
                        Directory.CreateDirectory(imageDirectorySmall);
                        Directory.CreateDirectory(imageDirectoryMiddle);
                        Directory.CreateDirectory(imageDirectoryBig);
                    }

                    // resmi sunucuya kaydet
                    image.SaveAs(Path.Combine(imageDirectory, fileName));

                    // resmi küçük boyutta kaydet
                    ImageManager.SaveResizedImage(Image.FromFile(Path.Combine(imageDirectory, fileName)), new Size(180, 180), imageDirectorySmall, fileName);
                    ImageManager.SaveResizedImage(Image.FromFile(Path.Combine(imageDirectory, fileName)), new Size(360, 360), imageDirectoryMiddle, fileName);
                    ImageManager.SaveResizedImage(Image.FromFile(Path.Combine(imageDirectory, fileName)), new Size(720, 720), imageDirectoryBig, fileName);

                    var post = new Post();

                    post.InsertDate = DateTime.Now;
                    post.InsertUserId = CustomMembership.CurrentUser().Id;
                    post.IsActive = model.IsActive;
                    post.ImgUrl = Path.Combine("Content/Images/uploads/Post/", fileName);
                    post.ImgUrlSmall = Path.Combine("Content/Images/uploads/Post/Small", fileName);
                    post.ImgUrlMiddle = Path.Combine("Content/Images/uploads/Post/Middle", fileName);
                    post.ImgUrlBig = Path.Combine("Content/Images/uploads/Post/Big", fileName);
                    post.AuthorId = model.AuthorId;
                    post.CategoryId = model.CategoryId;
                    post.Content = model.Content;
                    post.Description = model.Description;
                    post.PostPositionId = model.PositionId;
                    post.Slug = StringManager.ToSlug(model.Title);
                    post.Source = model.Source;
                    post.TagNames = model.TagNames;
                    post.Title = model.Title;

                    foreach (var PictureGaleryId in model.SelectedPictureGaleryIds)
                    {
                        post.Galeries.Add(_PictureGaleryService.Find(PictureGaleryId));
                    }

                    var selectedTagNames = model.TagNames.Split(',');
                    model.ListTagNames = _tagService.GetAll().Select(x => x.Name).ToList();

                    foreach (var tagName in selectedTagNames)
                    {
                        // etiket sistemde kayıtlı ise
                        if (model.ListTagNames.Contains(tagName))
                        {
                            post.Tags.Add(_tagService.GetAll().FirstOrDefault(x => x.Name == tagName));
                        }
                        else
                        {
                            // etiket sistemde kayıtlı degil ise
                            var newTag = new Tag
                            {
                                Name = tagName,
                                Description = tagName,
                                InsertDate = DateTime.Now,
                                InsertUserId = CustomMembership.CurrentUser().Id,
                                IsActive = true,
                                Slug = StringManager.ToSlug(tagName)
                            };
                            _tagService.Insert(newTag);
                            post.Tags.Add(newTag);
                        }
                    }

                    try
                    {
                        _postService.Insert(post);
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
            }

            model.Categories = _categoryService.GetAll();
            // tagnames yukarıda set ediliyor...
            model.Positions = _postService.GetAllPositions();
            model.Galeries = _PictureGaleryService.GetAll();
            model.Authors = _membershipService.GetAllUsers();

            return View(model);
        }

        public ActionResult EditPost(int id)
        {
            var model = new PostModel();
            var post = _postService.Find(id);

            model.AuthorId = post.AuthorId;
            model.Authors = _membershipService.GetAllUsers();
            model.Categories = _categoryService.GetAll();
            model.CategoryId = post.CategoryId;
            model.Content = post.Content;
            model.Description = post.Description;
            model.Galeries = _PictureGaleryService.GetAll();
            model.Id = post.Id;
            model.ImgUrl = post.ImgUrlSmall;
            model.IsActive = post.IsActive;
            model.ListTagNames = _tagService.GetAll().Select(x => x.Name).ToList();
            model.PositionId = post.PostPositionId;
            model.Positions = _postService.GetAllPositions();
            model.SelectedPictureGaleryIds = post.Galeries.Select(x => x.Id).ToList();
            model.Source = post.Source;
            model.TagNames = post.TagNames;
            model.Title = post.Title;

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditPost(PostModel model)
        {
            var post = _postService.Find(model.Id);
            ModelState.Remove("Img");

            if (ModelState.IsValid)
            {
                if (model.Img != null)
                {
                    var image = model.Img;
                    var fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(image.FileName);
                    var imageDirectory = Server.MapPath("~/Content/Images/uploads/Post");
                    var imageDirectorySmall = Server.MapPath("~/Content/Images/uploads/Post/Small");
                    var imageDirectoryMiddle = Server.MapPath("~/Content/Images/uploads/Post/Middle");
                    var imageDirectoryBig = Server.MapPath("~/Content/Images/uploads/Post/Big");

                    // dizin yoksa oluştur.
                    if (!Directory.Exists(imageDirectory))
                    {
                        Directory.CreateDirectory(imageDirectory);
                        Directory.CreateDirectory(imageDirectorySmall);
                        Directory.CreateDirectory(imageDirectoryMiddle);
                        Directory.CreateDirectory(imageDirectoryBig);
                    }

                    // resmi sunucuya kaydet
                    image.SaveAs(Path.Combine(imageDirectory, fileName));

                    // resmi küçük boyutta kaydet
                    ImageManager.SaveResizedImage(Image.FromFile(Path.Combine(imageDirectory, fileName)), new Size(180, 180), imageDirectorySmall, fileName);
                    ImageManager.SaveResizedImage(Image.FromFile(Path.Combine(imageDirectory, fileName)), new Size(360, 360), imageDirectoryMiddle, fileName);
                    ImageManager.SaveResizedImage(Image.FromFile(Path.Combine(imageDirectory, fileName)), new Size(720, 720), imageDirectoryBig, fileName);

                    post.ImgUrl = Path.Combine("Content/Images/uploads/Post/", fileName);
                    post.ImgUrlSmall = Path.Combine("Content/Images/uploads/Post/Small", fileName);
                    post.ImgUrlMiddle = Path.Combine("Content/Images/uploads/Post/Middle", fileName);
                    post.ImgUrlBig = Path.Combine("Content/Images/uploads/Post/Big", fileName);
                }

                post.IsActive = model.IsActive;
                post.AuthorId = model.AuthorId;
                post.CategoryId = model.CategoryId;
                post.Content = model.Content;
                post.Description = model.Description;
                post.PostPositionId = model.PositionId;
                post.Slug = StringManager.ToSlug(model.Title);
                post.Source = model.Source;
                post.TagNames = model.TagNames;
                post.Title = model.Title;
                post.UpdateDate = DateTime.Now;
                post.UpdateUserId = CustomMembership.CurrentUser().Id;

                post.Galeries.Clear();
                foreach (var PictureGaleryId in model.SelectedPictureGaleryIds)
                {
                    post.Galeries.Add(_PictureGaleryService.Find(PictureGaleryId));
                }

                var selectedTagNames = model.TagNames.Split(',');
                model.ListTagNames = _tagService.GetAll().Select(x => x.Name).ToList();

                post.Tags.Clear();
                foreach (var tagName in selectedTagNames)
                {
                    // etiket sistemde kayıtlı ise
                    if (model.ListTagNames.Contains(tagName))
                    {
                        post.Tags.Add(_tagService.GetAll().FirstOrDefault(x => x.Name == tagName));
                    }
                    else
                    {
                        // etiket sistemde kayıtlı degil ise
                        var newTag = new Tag
                        {
                            Name = tagName,
                            Description = tagName,
                            InsertDate = DateTime.Now,
                            InsertUserId = CustomMembership.CurrentUser().Id,
                            IsActive = true,
                            Slug = StringManager.ToSlug(tagName)
                        };
                        _tagService.Insert(newTag);
                        post.Tags.Add(newTag);
                    }
                }

                try
                {
                    _postService.Update(post);
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


            model.Categories = _categoryService.GetAll();
            // tagnames yukarıda set ediliyor...
            model.Positions = _postService.GetAllPositions();
            model.Galeries = _PictureGaleryService.GetAll();
            model.Authors = _membershipService.GetAllUsers();

            return View(model);
        }

        public ActionResult DeletePost(int id)
        {
            var post = _postService.Find(id);
            try
            {
                _postService.Delete(post);
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