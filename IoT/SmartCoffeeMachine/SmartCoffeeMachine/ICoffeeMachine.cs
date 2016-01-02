using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SmartCoffeeMachine
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "ICoffeeMachine" в коде и файле конфигурации.
    [ServiceContract]
    public interface ICoffeeMachine
    {
        [OperationContract]
        void MakeDrink(Order order);

        [OperationContract]
        QueueItem WaitFor(long coffeeMachineId);
    }
}
