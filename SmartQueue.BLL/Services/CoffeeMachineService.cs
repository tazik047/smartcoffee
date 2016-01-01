using System.Collections.Generic;
using System.Linq;
using SmartQueue.Model.Entities;
using SmartQueue.Model.Repositories;
using SmartQueue.Model.Services;

namespace SmartQueue.BLL.Services
{
    class CoffeeMachineService : ICoffeeMachineService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CoffeeMachineService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void AddCoffeeMachie(CoffeeMachine coffeeMachine)
        {
            _unitOfWork.CoffeeMachineRepository.Add(coffeeMachine);
            _unitOfWork.Save();
        }

        public void EditCoffeeMachie(CoffeeMachine coffeeMachine)
        {
            var originMachine = _unitOfWork.CoffeeMachineRepository.Get(coffeeMachine.Id);
            originMachine.Name = coffeeMachine.Name;
            originMachine.ServiceStaff =
                _unitOfWork.UserRepository.Get(u => coffeeMachine.ServiceStaff.Any(s => s.Id == u.Id)).ToList();
            originMachine.Position = coffeeMachine.Position;
            _unitOfWork.CoffeeMachineRepository.Edit(originMachine);
            _unitOfWork.Save();
        }

        public CoffeeMachine GetCoffeeMachie(long id)
        {
            return _unitOfWork.CoffeeMachineRepository.Get(id);
        }

        public IEnumerable<CoffeeMachine> GetAllCoffeeMachines()
        {
            return _unitOfWork.CoffeeMachineRepository.Get();
        }

        public IEnumerable<CoffeeMachine> GetAllCoffeeMachines(long companyId)
        {
            return _unitOfWork.CoffeeMachineRepository.Get(c => c.CompanyId == companyId).ToList();
        }
    }
}
