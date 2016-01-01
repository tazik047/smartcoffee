using System;
using System.Collections.Generic;
using System.Linq;
using SmartQueue.Model.Entities;
using SmartQueue.Model.Repositories;
using SmartQueue.Model.Services;
using SmartQueue.Utils.Sequrity;

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

        public void RegisterUser(User user, string role)
        {
            user.Roles = _unitOfWork.RoleRepository.Get(r => r.Name.Equals(role)).ToList();
            user.Password = Encrypter.HashText(user.Password);
            _unitOfWork.UserRepository.Add(user);
            _unitOfWork.Save();
        }

        public User Login(string login, string password)
        {
            password = Encrypter.HashText(password);
            return _unitOfWork.UserRepository.Get(login, password);
        }

        public bool ChangePassword(long id, string oldPassword, string newPassword)
        {
            var user = _unitOfWork.UserRepository.Get(id);
            
            if (!user.Password.Equals(Encrypter.HashText(oldPassword)))
            {
                return false;
            }

            user.Password = Encrypter.HashText(newPassword);
            _unitOfWork.UserRepository.Edit(user);
            _unitOfWork.Save();

            return true;
        }

        public void SetFavouriteCofeeMachine(CoffeeMachine coffeeMachine)
        {
            throw new NotImplementedException();
        }

        public User GetUserOrDefault(string login)
        {
            return _unitOfWork.UserRepository.Get(login);
        }

        public User GetUserOrDefault(long id)
        {
            return _unitOfWork.UserRepository.Get(id);
        }

        public bool IsBanned(User user)
        {
            return !_unitOfWork.UserRepository.Get(user.Id).IsActive;
        }

        public void BanUser(long id)
        {
            var user = _unitOfWork.UserRepository.Get(id);
            user.IsActive = false;
            _unitOfWork.UserRepository.Edit(user);
            _unitOfWork.Save();
        }

        public void UpdateUser(User user)
        {
            var originUser = GetUserOrDefault(user.Id);
            originUser.Name = user.Name;
            originUser.Surname = user.Surname;
            originUser.Email = user.Email;
            originUser.ContentType = user.ContentType;
            _unitOfWork.UserRepository.Edit(originUser);
            _unitOfWork.Save();
        }

        public IEnumerable<User> GetSelectedUsers(List<long> userIds)
        {
            return _unitOfWork.UserRepository.Get(u => userIds.Contains(u.Id)).ToList();
        }
    }
}
