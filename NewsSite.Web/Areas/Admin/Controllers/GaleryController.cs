using NewsSite.Core.Database.Tables;
using NewsSite.Data.UnitOfWork;
using NewsSite.Service.GaleryServices;
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
    public class GaleryController : AdminController
    {
        private readonly IGaleryService _galeryService;

        public GaleryController(IUnitOfWork uow, IGaleryService galeryService)
            : base(uow)
        {
            _galeryService = galeryService;
        }

        #region galery
        public ActionResult Index()
        {
            var galeries = _galeryService.GetAll();

            return View(galeries.OrderBy(x => x.Id));
        }

        public ActionResult AddGalery()
        {
            var model = new GaleryModel();
            model.IsActive = true;

            return View(model);
        }

        [HttpPost]
        public ActionResult AddGalery(GaleryModel model)
        {
            var galery = new Galery();

            if (ModelState.IsValid)
            {
                galery.Description = model.Description;
                galery.InsertDate = DateTime.Now;
                galery.InsertUserId = CustomMembership.CurrentUser().Id;
                galery.IsActive = model.IsActive;
                galery.Name = model.Name;

                try
                {
                    _galeryService.Insert(galery);
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

        public ActionResult EditGalery(int id)
        {
            var model = new GaleryModel();
            var galery = _galeryService.Find(id);

            model.Description = galery.Description;
            model.Id = galery.Id;
            model.IsActive = galery.IsActive;
            model.Name = galery.Name;

            return View(model);
        }

        [HttpPost]
        public ActionResult EditGalery(GaleryModel model)
        {
            var galery = _galeryService.Find(model.Id);

            if (ModelState.IsValid)
            {
                galery.Description = model.Description;
                galery.IsActive = model.IsActive;
                galery.Name = model.Name;
                galery.Slug = StringManager.ToSlug(model.Name);
                galery.UpdateUserId = CustomMembership.CurrentUser().Id;
                galery.UpdateDate = DateTime.Now;

                try
                {
                    _galeryService.Update(galery);
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

        public ActionResult DeleteGalery(int id)
        {
            var galery = _galeryService.Find(id);
            try
            {
                _galeryService.Delete(galery);
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

        #region galery images
        public ActionResult GaleryImages(int galeryId)
        {
            var galery = _galeryService.Find(galeryId);
            var model = new GaleryImageModel();

            model.Galery = galery;
            model.Order = 0;

            return View(model);
        }

        [HttpPost]
        public ActionResult AddGaleryImage(GaleryImageModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.GaleryImg.ContentLength > 0)
                {
                    var image = model.GaleryImg;
                    var fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(image.FileName);
                    var imageDirectory = Server.MapPath("~/Content/Images/uploads/Galery/" + model.Galery.Id);
                    var imageDirectorySmall = Server.MapPath("~/Content/Images/uploads/Galery/" + model.Galery.Id + "/Small");
                    var imageDirectoryMiddle = Server.MapPath("~/Content/Images/uploads/Galery/" + model.Galery.Id + "/Middle");
                    var imageDirectoryBig = Server.MapPath("~/Content/Images/uploads/Galery/" + model.Galery.Id + "/Big");

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

                    var galeryImage = new GaleryImage();

                    galeryImage.ContentSize = image.ContentLength;
                    galeryImage.ContentType = image.ContentType;
                    galeryImage.FileName = fileName;
                    galeryImage.GaleryId = model.Galery.Id;
                    galeryImage.InsertDate = DateTime.Now;
                    galeryImage.InsertUserId = CustomMembership.CurrentUser().Id;
                    galeryImage.IsActive = true;
                    galeryImage.Order = model.Order;
                    galeryImage.ImgUrl = Path.Combine("Content/Images/uploads/Galery/" + model.Galery.Id, fileName);
                    galeryImage.ImgUrlSmall = Path.Combine("Content/Images/uploads/Galery/" + model.Galery.Id + "/Small", fileName);
                    galeryImage.ImgUrlMiddle = Path.Combine("Content/Images/uploads/Galery/" + model.Galery.Id + "/Middle", fileName);
                    galeryImage.ImgUrlBig = Path.Combine("Content/Images/uploads/Galery/" + model.Galery.Id + "/Big", fileName);

                    try
                    {
                        _galeryService.Insert(galeryImage);
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

            return RedirectToAction("GaleryImages", new { galeryId = model.Galery.Id });
        }

        public ActionResult DeleteGaleryImage(int id)
        {
            var galeryImage = _galeryService.FindGaleryImage(id);
            var model = new GaleryImageModel();
            model.Galery = galeryImage.Galery;

            try
            {
                _galeryService.Delete(galeryImage);
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

            return View("GaleryImages", model);
        }
        #endregion
    }
}