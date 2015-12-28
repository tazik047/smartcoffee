using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SmartQueue.Authorization.Infrastructure;
using SmartQueue.Authorization.Interfaces;
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
            var loginedUser = _authorization.Login(user.Login, user.Password, user.RememberMe);
            if (loginedUser != null)
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("","Логин или пароль введен неверно.");
            return View(user);
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Logout()
        {
            _authorization.LogOut();
            return RedirectToAction("Index", "Home");
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