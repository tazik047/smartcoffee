using System.Linq;
using System.Security.Principal;
using SmartQueue.Model.Services;

namespace SmartQueue.Authorization
{
    class UserProvider : IPrincipal
    {
        private readonly UserIdentity _userIdentity;

        public UserProvider(string name, IUserService service)
        {
            _userIdentity = new UserIdentity(name, service);
        }

        public bool IsInRole(string role)
        {
            var roles = role.Split(',').Select(r => r.Trim()).ToList();
            return _userIdentity.User != null && _userIdentity.User.Roles.Any(userRole => roles.Any(r => r == userRole.Name));
        }

        public IIdentity Identity
        {
            get { return _userIdentity; }
        }

        public override string ToString()
        {
            return _userIdentity.Name;
        }

    }
}
