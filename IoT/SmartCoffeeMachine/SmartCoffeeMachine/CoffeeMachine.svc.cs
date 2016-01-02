using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SmartCoffeeMachine
{
    public class CoffeeMachine : ICoffeeMachine
    {
        private readonly CoffeeMachineWorker _worker;

        public CoffeeMachine()
        {
            _worker = CoffeeMachineWorker.GetInstance();
        }

        public void MakeDrink(Order order)
        {
            _worker.AddToQueue(order);
        }

        public QueueItem WaitFor(long coffeeMachineId)
        {
            return _worker.TimeForEnd(coffeeMachineId);
        }
    }
}
