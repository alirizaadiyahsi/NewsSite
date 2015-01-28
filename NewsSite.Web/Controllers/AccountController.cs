using FormsAuthenticationExtensions;
using NewsSite.Core.Application;
using NewsSite.Core.Database.Tables;
using NewsSite.Data.UnitOfWork;
using NewsSite.Service.MembershipServices;
using NewsSite.Utilities;
using NewsSite.Web.Framework.Controllers;
using NewsSite.Web.Framework.Membership;
using NewsSite.Web.Models;
using System;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Security;

namespace NewsSite.Web.Controllers
{
    public class AccountController : PublicController
    {
        private readonly IMembershipService _membershipService;

        public AccountController(IUnitOfWork uow, IMembershipService membershipService)
            : base(uow)
        {
            _membershipService = membershipService;
        }

        public ActionResult Login(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model, string ReturnUrl)
        {
            var user = _membershipService.FindByUserNameAndPassword(model.UserName, model.Password);
            if (ModelState.IsValid && user != null)
            {
                if (!user.IsConfirmed)
                {
                    TempData[Keys.EmailConfirm] = "E-posta adresiniz onaylı değildir. Lütfen e-posta adresinizdeki linki kullanarak e-posta adresinizi onaylayınız.";

                    return View();
                }

                var ticketData = new NameValueCollection
                {
                    { Keys.Email, user.Email },
                    { Keys.RealName, user.RealName },
                    { Keys.Id, user.Id.ToString() }
                };

                new FormsAuthentication().SetAuthCookie(model.UserName, model.RememberMe, ticketData);

                return RedirectToLocal(ReturnUrl);
            }
            else
            {
                ModelState.AddModelError("", "Kullanıcı adı ve ya şifre geçersiz!");
            }

            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = new User();

                    if (model.Img != null && model.Img.ContentLength > 0)
                    {
                        var image = model.Img;
                        var fileName = model.UserName + "_" + Guid.NewGuid() + System.IO.Path.GetExtension(image.FileName);

                        var imageDirectory = Server.MapPath("~/Content/Images/uploads/Users/" + model.UserName);
                        var imageDirectoryBig = Server.MapPath("~/Content/Images/uploads/Users/" + model.UserName + "/Big");
                        var imageDirectoryMiddle = Server.MapPath("~/Content/Images/uploads/Users/" + model.UserName + "/Middle");
                        var imageDirectorySmall = Server.MapPath("~/Content/Images/uploads/Users/" + model.UserName + "/Small");

                        // dizin yoksa oluştur.
                        if (!Directory.Exists(imageDirectory))
                        {
                            Directory.CreateDirectory(imageDirectory);
                            Directory.CreateDirectory(imageDirectoryBig);
                            Directory.CreateDirectory(imageDirectoryMiddle);
                            Directory.CreateDirectory(imageDirectorySmall);
                        }

                        // resmi sunucuya kaydet
                        image.SaveAs(Path.Combine(imageDirectory, fileName));

                        // resmi küçük boyutta kaydet
                        ImageManager.SaveResizedImage(Image.FromFile(Path.Combine(imageDirectory, fileName)), new Size(150, 150), imageDirectorySmall, fileName);
                        ImageManager.SaveResizedImage(Image.FromFile(Path.Combine(imageDirectory, fileName)), new Size(450, 450), imageDirectoryMiddle, fileName);
                        ImageManager.SaveResizedImage(Image.FromFile(Path.Combine(imageDirectory, fileName)), new Size(750, 750), imageDirectoryBig, fileName);

                        user.ImgUrl = Path.Combine("Content/Images/uploads/Users/" + model.UserName + "/", fileName);
                        user.ImgUrlBig = Path.Combine("Content/Images/uploads/Users/" + model.UserName + "/Big/", fileName);
                        user.ImgUrlMiddle = Path.Combine("Content/Images/uploads/Users/" + model.UserName + "/Middle/", fileName);
                        user.ImgUrlSmall = Path.Combine("Content/Images/uploads/Users/" + model.UserName + "/Small/", fileName);
                    }

                    user.ConfirmationId = Guid.NewGuid();
                    user.IsConfirmed = true;
                    user.LastLoginDate = DateTime.Now;
                    user.LastLoginIP = Request.UserHostAddress;
                    user.Password = model.Password;
                    user.Email = model.Email;
                    user.UserName = model.UserName;
                    user.IsActive = true;
                    user.InsertDate = DateTime.Now;
                    user.UpdateDate = DateTime.Now;
                    user.InsertUserId = 0;
                    user.UpdateUserId = 0;
                    user.Description = model.Description;
                    user.RealName = String.IsNullOrEmpty(model.RealName) ? model.UserName : model.RealName;
                    user.Website = model.Website;

                    _membershipService.Insert(user);
                    _uow.SaveChanges();
                    //_membershipService.SendConfirmationMail(user, Request.Url.GetLeftPart(UriPartial.Authority));

                    FormsAuthentication.SetAuthCookie(model.UserName, true);

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Kullanıcı oluşturma başarısız! Hata: " + ex.Message);
                }
            }

            return View(model);
        }

        // RegisterModel içerisindeki Email alanını
        // RemoteAttribute ile kontrol eder
        public JsonResult ValidateEmail(string Email)
        {
            var result = _membershipService.ValidateEmail(Email);

            if (result)
            {
                return Json("Girdiğiniz e-posta adresi sistemde zaten mevcut!", JsonRequestBehavior.AllowGet);
            }

            return Json(!result, JsonRequestBehavior.AllowGet);
        }

        // RegisterModel içerisindeki UserName alanını
        // RemoteAttribute ile kontrol eder
        public JsonResult ValidateUserName(string UserName)
        {
            var result = _membershipService.ValidateUserName(UserName);

            if (result)
            {
                return Json("Girdiğiniz kullanıcı adı sistemde zaten mevcut!", JsonRequestBehavior.AllowGet);
            }

            return Json(!result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult ConfirmUser(Guid confirmationId)
        {
            if (string.IsNullOrEmpty(confirmationId.ToString()) || (!Regex.IsMatch(confirmationId.ToString(),
                   @"[0-9a-f]{8}\-([0-9a-f]{4}\-){3}[0-9a-f]{12}")))
            {
                TempData[Keys.EmailConfirm] = "Hesap geçerli değil. Lütfen e-posta adresinizdeki linke tekrar tıklayınız.";

                return View();
            }
            else
            {
                var user = _membershipService.FindByConfirmationId(confirmationId);

                if (!user.IsConfirmed)
                {
                    user.IsConfirmed = true;
                    _membershipService.Update(user);
                    _uow.SaveChanges();

                    FormsAuthentication.SetAuthCookie(user.UserName, true);
                    TempData[Keys.EmailConfirm] = "E-posta adresinizi onayladığınız için teşekkürler. Artık sitemize üyesiniz.";

                    return RedirectToAction("Wellcome");
                }
                else
                {
                    TempData[Keys.EmailConfirm] = "E-posta adresiniz zaten onaylı. Giriş yapabilirsiniz.";

                    return RedirectToAction("GirisYap");
                }
            }
        }

        #region private methods
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion
    }
}