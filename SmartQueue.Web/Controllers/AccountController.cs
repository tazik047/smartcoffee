using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
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
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult RegisterUser(RegisterUserViewModel model)
        {
            return View();
        }

        public ActionResult Logout()
        {
            _authorization.LogOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Manage()
        {
            return View();
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
            var path = Server.MapPath("~/Images/" + name);
            return File(path, contentType);
        }
    }
}