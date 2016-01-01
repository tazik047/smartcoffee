using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using AutoMapper;
using SmartQueue.Authorization.Infrastructure;
using SmartQueue.Authorization.Interfaces;
using SmartQueue.Model.Entities;
using SmartQueue.Model.Services;
using SmartQueue.Web.Models;

namespace SmartQueue.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ISmartQueueServices _smartQueueServices;

        private readonly IAuthorization _authorization;

        public AccountController(ISmartQueueServices smartQueueServices, IAuthorization authorization)
        {
            _smartQueueServices = smartQueueServices;
            _authorization = authorization;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            try
            {
                var loginedUser = _authorization.Login(user.Login, user.Password, user.RememberMe);

                if (loginedUser != null)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("","Логин или пароль введен неверно.");
            }
            catch (UnauthorizedAccessException e)
            {
                ModelState.AddModelError("","Ваша учетная запись не активирована.");
            }
            
            return View(user);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult RegisterCompany()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult RegisterCompany(RegisterCompanyViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = Mapper.Map<User>(model);
                    _authorization.RegisterUser(user, "Director");
                }
                catch (UnauthorizedAccessException e)
                {
                    TempData["Registered"] = "Ваша учетная запись была создана и ожидает подтверждения администратора";
                    return RedirectToAction("Login");
                }

                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult RegisterUser()
        {
            var user = new RegisterUserViewModel();
            FillCompanies(user);
            return View(user);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult RegisterUser(RegisterUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = Mapper.Map<User>(model);
                    _authorization.RegisterUser(user, "User");
                }
                catch (UnauthorizedAccessException e)
                {
                    TempData["Registered"] = "Ваша учетная запись была создана и ожидает подтверждения директором вашей фирмы";
                    return RedirectToAction("Login");
                }

                return RedirectToAction("Index", "Home");
            }
            FillCompanies(model);
            return View(model);
        }

        public ActionResult Logout()
        {
            _authorization.LogOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (_smartQueueServices.UserService.ChangePassword(User.Identity.GetUser().Id, model.OldPassword, model.Password))
            {
                TempData["SuccessMessage"] = "Пароль успешно сменен.";
                return RedirectToAction("Details");
            }
            ModelState.AddModelError("OldPassword", "Пароль введен неверно");
            return View();
        }

        public ActionResult Details()
        {
            var user = User.Identity.GetUser();
            return View(Mapper.Map<UserViewModel>(user));
        }

        [HttpGet]
        public ActionResult Manage()
        {
            var user = User.Identity.GetUser();
            return View(Mapper.Map<ManageUserViewModel>(user));
        }

        [HttpPost]
        public ActionResult Manage(ManageUserViewModel user, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                var originUser = User.Identity.GetUser();
                Mapper.Map(user, originUser);
                if (file != null && file.ContentType.StartsWith("image"))
                {
                    originUser.ContentType = file.ContentType;
                    file.SaveAs(GetPathToPhoto(user.Email));
                }
                _smartQueueServices.UserService.UpdateUser(originUser);
                TempData["SuccessMessage"] = "Данные успешно сохранены";
                return RedirectToAction("Details");
            }
            return View(user);
        }

        [AllowAnonymous]
        public ActionResult Photo(long id)
        {
            var user = _smartQueueServices.UserService.GetUserOrDefault(id);
            string name, contentType;
            if (string.IsNullOrEmpty(user.ContentType))
            {
                contentType = "image/jpeg";
                name = "default";
            }
            else
            {
                name = user.Email;
                contentType = user.ContentType;
            }
            var path = GetPathToPhoto(name);
            return File(path, contentType);
        }

        private void FillCompanies(RegisterUserViewModel user)
        {
            user.Companies =
                _smartQueueServices.CompanyService.GetAllCompanies()
                    .Select(c => new SelectListItem {Text = c.Name, Value = c.Id.ToString()})
                    .ToList();
        }

        private string GetPathToPhoto(string name)
        {
            return Server.MapPath("~/Images/" + name);
        }
    }
}