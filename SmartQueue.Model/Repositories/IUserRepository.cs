using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartQueue.Model.Entities;

namespace SmartQueue.Model.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        User Get(string login);

        User Get(string login, string password);
    }
}
