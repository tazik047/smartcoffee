using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartQueue.Model.Entities;

namespace SmartQueue.Model.Services
{
    public interface ICoffeeMachineService
    {
        void AddCoffeeMachie(CoffeeMachine coffeeMachine);

        void EditCoffeeMachie(CoffeeMachine coffeeMachine);

        CoffeeMachine GetCoffeeMachie(long id);

        IEnumerable<CoffeeMachine> GetAllCoffeeMachines();

        IEnumerable<CoffeeMachine> GetAllCoffeeMachines(long companyId);
    }
}
