using SmartQueue.Model.Repositories;
using SmartQueue.Model.Services;

namespace SmartQueue.BLL.Services
{
    public class SmartQueueServices : ISmartQueueServices
    {
        private readonly IUnitOfWork _unitOfWork;

        private IUserService _userService;

        private IPreferencesService _preferencesService;

        public SmartQueueServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IUserService UserService
        {
            get { return _userService ?? (_userService= new UserService(_unitOfWork)); }
        }

        public IPreferencesService PreferencesService
        {
            get { return _preferencesService ?? (_preferencesService = new PreferencesService(_unitOfWork)); }
        }
    }
}
