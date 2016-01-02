using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartQueue.Model.Entities;

namespace SmartQueue.Model.Services
{
    public interface IQueueService
    {
        void AddToQueue(Order order);

        void RemoveFromQueue(long userId);

        IEnumerable<User> GetAllFromQueue(long coffeeMachineId);

        TimeSpan TimeLeft(long userId);

        bool IsWait(long userId);
    }
}
