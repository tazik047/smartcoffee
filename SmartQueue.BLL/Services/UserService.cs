using System;
using SmartQueue.Model.Entities;
using SmartQueue.Model.Repositories;
using SmartQueue.Model.Services;

namespace SmartQueue.BLL.Services
{
    class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void ActivateUser(long id)
        {
            var user = _unitOfWork.UserRepository.Get(id);
            user.IsActive = true;
            _unitOfWork.UserRepository.Edit(user);
            _unitOfWork.Save();
        }

        public void RegisterUser(User user)
        {
            _unitOfWork.UserRepository.Add(user);
            _unitOfWork.Save();
        }

        public User Login(string login, string password)
        {
            return _unitOfWork.UserRepository.Get(login, password);
        }

        public void ChangePassword(string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public void SetFavouriteCofeeMachine(CoffeeMachine coffeeMachine)
        {
            throw new NotImplementedException();
        }

        public User GetUserOrDefault(string login)
        {
            return _unitOfWork.UserRepository.Get(login);
        }

        public bool IsBanned(User user)
        {
            return !_unitOfWork.UserRepository.Get(user.Id).IsActive;
        }
    }
}
