using System.Security.Principal;
using SmartQueue.Model.Entities;

namespace SmartQueue.Authorization.Infrastructure
{
    public static class IdentityExtension
    {
        public static string GetName(this IIdentity identity)
        {
            var user = identity as UserIdentity;
            if (user != null)
            {
                return user.User == null ? user.Name : user.User.Login;
            }
            return string.Empty;
        }

        public static User GetUser(this IIdentity identity)
        {
            var userIdentity = identity as UserIdentity;
            return userIdentity == null ? null : userIdentity.User;
        }

        public static bool IsBanned(this IIdentity identity)
        {
            var userIdentity = identity as UserIdentity;
            if (userIdentity == null || !userIdentity.IsAuthenticated)
            {
                return false;
            }
            return userIdentity.IsBanned;
        }
    }
}
