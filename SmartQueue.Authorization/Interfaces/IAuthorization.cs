using System;
using System.Security.Principal;
using System.Web;
using SmartQueue.Model.Entities;

namespace SmartQueue.Authorization.Interfaces
{
    public interface IAuthorization
    {
        HttpContext HttpContext { get; set; }

        User Login(string login, string password, bool isPersistent);

        void RegisterUser(User user);

        void LogOut();

        User GetUserOrDefault(string name);

        IPrincipal CurrentUser { get; }
    }

}
