using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using SmartQueue.Authorization.Interfaces;
using SmartQueue.Model.Entities;
using SmartQueue.Model.Services;

namespace SmartQueue.Authorization
{
    public class SmartQueueAuthorization : IAuthorization
    {
        private IPrincipal _currentUser;

        public HttpContext HttpContext { get; set; }

        private readonly IUserService _service;

        public SmartQueueAuthorization(ISmartQueueServices services)
        {
            _service = services.UserService;
        }

        public User Login(string userName, string password, bool isPersistent)
        {
            var retUser = _service.Login(userName, password);
            if (retUser != null && retUser.IsActive)
                CreateCookie(userName, isPersistent);
            else if (retUser != null && !retUser.IsActive)
            {
                throw new UnauthorizedAccessException();
            }
            return retUser;
        }

        public void RegisterUser(User user, string role)
        {
            var originPassword = user.Password;
            _service.RegisterUser(user, role);
            Login(user.Login, originPassword, false);
        }

        private void CreateCookie(string userName, bool isPersistent = false)
        {
            var ticket = new FormsAuthenticationTicket(
                    1, userName, DateTime.UtcNow,
                    DateTime.UtcNow.Add(FormsAuthentication.Timeout),
                    isPersistent, string.Empty,
                    FormsAuthentication.FormsCookiePath);

            var encruptTicket = FormsAuthentication.Encrypt(ticket);
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName)
            {
                Value = encruptTicket,
                Expires = DateTime.UtcNow.Add(FormsAuthentication.Timeout)
            };
            HttpContext.Response.Cookies.Set(authCookie);
        }


        public void LogOut()
        {
            var httpCookie = HttpContext.Response.Cookies[FormsAuthentication.FormsCookieName];
            if (httpCookie != null)
                httpCookie.Value = string.Empty;
        }

        public User GetUserOrDefault(string name)
        {
            return _service.GetUserOrDefault(name);
        }

        public IPrincipal CurrentUser
        {
            get
            {
                if (_currentUser == null)
                {
                    try
                    {
                        var authCookie = HttpContext.Request.Cookies.Get(FormsAuthentication.FormsCookieName);
                        if (authCookie != null && !string.IsNullOrEmpty(authCookie.Value))
                        {
                            var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                            _currentUser = new UserProvider(ticket.Name, _service);
                        }
                        else
                        {
                            _currentUser = new UserProvider(null, null);
                        }
                    }
                    catch (Exception)
                    {
                        _currentUser = new UserProvider(null, null);
                    }
                }
                return _currentUser;
            }
        }
    }
}
