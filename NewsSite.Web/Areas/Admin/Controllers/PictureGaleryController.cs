using NewsSite.Core.Database.Tables;
using NewsSite.Data.UnitOfWork;
using NewsSite.Service.PictureGaleryServices;
using NewsSite.Utilities;
using NewsSite.Web.Areas.Admin.Models;
using NewsSite.Web.Framework.Controllers;
using NewsSite.Web.Framework.Membership;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace NewsSite.Web.Areas.Admin.Controllers
{
    public class PictureGaleryController : PublicController
    {
        private readonly IPictureGaleryService _PictureGaleryService;

        public PictureGaleryController(IUnitOfWork uow, IPictureGaleryService PictureGaleryService)
            : base(uow)
        {
            _PictureGaleryService = PictureGaleryService;
        }

        #region PictureGalery
        public ActionResult Index()
        {
            var galeries = _PictureGaleryService.GetAll();

            return View(galeries.OrderBy(x => x.Id));
        }

        public ActionResult AddPictureGalery()
        {
            var model = new PictureGaleryModel();
            model.IsActive = true;

            return View(model);
        }

        [HttpPost]
        public ActionResult AddPictureGalery(PictureGaleryModel model)
        {
            var PictureGalery = new PictureGalery();

            if (ModelState.IsValid)
            {
                PictureGalery.Description = model.Description;
                PictureGalery.InsertDate = DateTime.Now;
                PictureGalery.InsertUserId = CustomMembership.CurrentUser().Id;
                PictureGalery.IsActive = model.IsActive;
                PictureGalery.Name = model.Name;

                try
                {
                    _PictureGaleryService.Insert(PictureGalery);
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

        public ActionResult EditPictureGalery(int id)
        {
            var model = new PictureGaleryModel();
            var PictureGalery = _PictureGaleryService.Find(id);

            model.Description = PictureGalery.Description;
            model.Id = PictureGalery.Id;
            model.IsActive = PictureGalery.IsActive;
            model.Name = PictureGalery.Name;

            return View(model);
        }

        [HttpPost]
        public ActionResult EditPictureGalery(PictureGaleryModel model)
        {
            var PictureGalery = _PictureGaleryService.Find(model.Id);

            if (ModelState.IsValid)
            {
                PictureGalery.Description = model.Description;
                PictureGalery.IsActive = model.IsActive;
                PictureGalery.Name = model.Name;
                PictureGalery.Slug = StringManager.ToSlug(model.Name);
                PictureGalery.UpdateUserId = CustomMembership.CurrentUser().Id;
                PictureGalery.UpdateDate = DateTime.Now;

                try
                {
                    _PictureGaleryService.Update(PictureGalery);
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

        public ActionResult DeletePictureGalery(int id)
        {
            var PictureGalery = _PictureGaleryService.Find(id);
            try
            {
                _PictureGaleryService.Delete(PictureGalery);
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
        #endregion

        #region PictureGalery images
        public ActionResult Pictures(int PictureGaleryId)
        {
            var PictureGalery = _PictureGaleryService.Find(PictureGaleryId);
            var model = new PictureModel();

            model.PictureGalery = PictureGalery;
            model.Order = 0;

            return View(model);
        }

        [HttpPost]
        public ActionResult AddPicture(PictureModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.PictureGaleryImg.ContentLength > 0)
                {
                    var image = model.PictureGaleryImg;
                    var fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(image.FileName);
                    var imageDirectory = Server.MapPath("~/Content/Images/uploads/PictureGalery/" + model.PictureGalery.Id);
                    var imageDirectorySmall = Server.MapPath("~/Content/Images/uploads/PictureGalery/" + model.PictureGalery.Id + "/Small");
                    var imageDirectoryMiddle = Server.MapPath("~/Content/Images/uploads/PictureGalery/" + model.PictureGalery.Id + "/Middle");
                    var imageDirectoryBig = Server.MapPath("~/Content/Images/uploads/PictureGalery/" + model.PictureGalery.Id + "/Big");

                    // create directory if not exist
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

                    var Picture = new Picture();

                    Picture.ContentSize = image.ContentLength;
                    Picture.ContentType = image.ContentType;
                    Picture.FileName = fileName;
                    Picture.PictureGaleryId = model.PictureGalery.Id;
                    Picture.InsertDate = DateTime.Now;
                    Picture.InsertUserId = CustomMembership.CurrentUser().Id;
                    Picture.IsActive = true;
                    Picture.Order = model.Order;
                    Picture.ImgUrl = Path.Combine("Content/Images/uploads/PictureGalery/" + model.PictureGalery.Id, fileName);
                    Picture.ImgUrlSmall = Path.Combine("Content/Images/uploads/PictureGalery/" + model.PictureGalery.Id + "/Small", fileName);
                    Picture.ImgUrlMiddle = Path.Combine("Content/Images/uploads/PictureGalery/" + model.PictureGalery.Id + "/Middle", fileName);
                    Picture.ImgUrlBig = Path.Combine("Content/Images/uploads/PictureGalery/" + model.PictureGalery.Id + "/Big", fileName);

                    try
                    {
                        _PictureGaleryService.Insert(Picture);
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
                }
            }

            return RedirectToAction("Pictures", new { PictureGaleryId = model.PictureGalery.Id });
        }

        public ActionResult DeletePicture(int id)
        {
            var Picture = _PictureGaleryService.FindPicture(id);
            var model = new PictureModel();
            model.PictureGalery = Picture.PictureGalery;

            try
            {
                _PictureGaleryService.Delete(Picture);
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

            return View("Pictures", model);
        }
        #endregion
    }
}