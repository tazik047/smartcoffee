using System.Security.Principal;
using SmartQueue.Model.Entities;
using SmartQueue.Model.Services;

namespace SmartQueue.Authorization
{
    class UserIdentity : IIdentity
    {
        private readonly IUserService _service;

        public User User { get; set; }

        public UserIdentity(string name, IUserService service)
        {
            if (service != null)
            {
                User = service.GetUserOrDefault(name);
                _service = service;
            }
        }
        
        public string AuthenticationType
        {
            get
            {
                return typeof(User).ToString();
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return User != null;
            }
        }

        public string Name
        {
            get
            {
                if (User != null)
                {
                    return User.Email;
                }
                return "Guest";
            }
        }

        public bool IsBanned
        {
            get
            {
                if (_service == null || User == null)
                {
                    return false;
                }
                return _service.IsBanned(User);
            }
        }
    }
}
