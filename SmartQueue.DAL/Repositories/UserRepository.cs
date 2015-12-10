using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartQueue.Model.Entities;
using SmartQueue.Model.Repositories;

namespace SmartQueue.DAL.Repositories
{
    class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(EfDbContext context) : base(context)
        {
        }

        public User Get(string login)
        {
            return Context.Users.FirstOrDefault(u => u.Login.Equals(login, StringComparison.CurrentCultureIgnoreCase));
        }

        public User Get(string login, string password)
        {
            return Context.Users
                .FirstOrDefault(u => u.Login.Equals(login, StringComparison.CurrentCultureIgnoreCase) && u.Password == password);
        }
    }
}
