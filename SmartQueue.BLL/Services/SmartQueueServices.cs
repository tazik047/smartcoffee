using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartQueue.Model.Repositories;
using SmartQueue.Model.Services;

namespace SmartQueue.BLL.Services
{
    public class SmartQueueServices : ISmartQueueServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private IUserService _userService;

        public SmartQueueServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IUserService UserService
        {
            get { return _userService ?? (_userService= new UserService(_unitOfWork)); }
        }
    }
}
