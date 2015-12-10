using Ninject;
using SmartQueue.Authorization;
using SmartQueue.Authorization.Interfaces;
using SmartQueue.BLL.Services;
using SmartQueue.DAL.Repositories;
using SmartQueue.Model.Repositories;
using SmartQueue.Model.Services;

namespace SmartQueue.Configuration
{
    public static class Configuration
    {
        public static void RegisterDependencyInjection(IKernel kernel)
        {
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>();
            kernel.Bind<ISmartQueueServices>().To<SmartQueueServices>();
            kernel.Bind<IAuthorization>().To<SmartQueueAuthorization>();
        }
    }
}
