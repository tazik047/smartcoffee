using SmartQueue.Model.Repositories;
using SmartQueue.Model.Services;

namespace SmartQueue.BLL.Services
{
    public class SmartQueueServices : ISmartQueueServices
    {
        private readonly IUnitOfWork _unitOfWork;

        private IUserService _userService;

        private IPreferencesService _preferencesService;

        private IRoleService _roleService;

        private ICompanyService _companyService;

        private ICoffeeMachineService _coffeeMachineService;

        private IQueueService _queueService;

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

        public IRoleService RoleService
        {
            get { return _roleService ?? (_roleService = new RoleService(_unitOfWork)); }
        }

        public ICompanyService CompanyService
        {
            get { return _companyService ?? (_companyService = new CompanyService(_unitOfWork)); }
        }

        public ICoffeeMachineService CoffeeMachineService
        {
            get { return _coffeeMachineService ??(_coffeeMachineService = new CoffeeMachineService(_unitOfWork)); }
        }

        public IQueueService QueueService
        {
            get { return _queueService ?? (_queueService = new QueueService(_unitOfWork)); }
        }
    }
}
