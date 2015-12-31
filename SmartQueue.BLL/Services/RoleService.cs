using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartQueue.Model.Entities;
using SmartQueue.Model.Repositories;
using SmartQueue.Model.Services;

namespace SmartQueue.BLL.Services
{
    class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Role GetDirectorRole()
        {
            return _unitOfWork.RoleRepository.Get(r => r.Name.Equals("Director")).First();
        }
    }
}
