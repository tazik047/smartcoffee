using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartQueue.Model.Repositories
{
    public interface IUnitOfWork
    {
        ICoffeeMachineRepository CoffeeMachineRepository { get; }

        ICompanyRepository CompanyRepository { get; }

        IRoleRepository RoleRepository { get; }

        IUserRepository UserRepository { get; }

        void Save();
    }
}
