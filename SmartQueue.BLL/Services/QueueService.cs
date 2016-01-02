using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SmartQueue.Model.Entities;
using SmartQueue.Model.Repositories;
using SmartQueue.Model.Services;

namespace SmartQueue.BLL.Services
{
    class QueueService : IQueueService
    {
        private IUnitOfWork _unitOfWork;

        private static readonly object Locker= new object();

        private static readonly ConcurrentDictionary<long, List<Order>> Queue = new ConcurrentDictionary<long, List<Order>>();

        static QueueService()
        {
            
        }

        /*void Waiter()
        {
            while (true)
            {
                var makingOrders = new List<Order>();
                lock (Locker)
                {
                    foreach (var orders in Queue)
                    {
                        if (orders.Value.Count > 0)
                        {
                            makingOrders.Add(orders.Value[0]);
                            orders.Value.RemoveAt(0);
                        }
                    }
                }
                Thread.Sleep(60000);
                //TODO Notify users
            }
        }*/

        public QueueService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void AddToQueue(Order order)
        {
            lock (Locker)
            {
                order.StartDate = DateTime.UtcNow;
                if (!Queue.ContainsKey(order.CoffeeMachineId))
                {
                    Queue[order.CoffeeMachineId] = new List<Order> {order};
                }
                else
                {
                    Queue[order.CoffeeMachineId].Add(order);
                }
            }
        }

        public void RemoveFromQueue(long userId)
        {
            foreach (var item in Queue)
            {
                var order = item.Value.FirstOrDefault(o => o.UserId == userId);
                if (order != null)
                {
                    item.Value.Remove(order);
                    return;
                }
            }
        }

        public IEnumerable<User> GetAllFromQueue(long coffeeMachineId)
        {
            return Queue[coffeeMachineId].Select(c => c.User).ToList();
        }

        public TimeSpan TimeLeft(long userId)
        {
            throw new NotImplementedException();
        }

        public bool IsWait(long userId)
        {
            return Queue.SelectMany(q => q.Value).Any(o => o.UserId == userId);
        }
    }
}
