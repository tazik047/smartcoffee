using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartQueue.Model.Entities;

namespace SmartQueue.Model.Services
{
    public interface IUserService
    {
        void ActivateUser(long id);

        void RegisterUser(User user, string role);

        User Login(string login, string password);

        void ChangePassword(string oldPassword, string newPassword);

        void SetFavouriteCofeeMachine(CoffeeMachine coffeeMachine);

        User GetUserOrDefault(string login);

        User GetUserOrDefault(long id);

        bool IsBanned(User user);

        void RegisterCompany(Company company);
    }
}
