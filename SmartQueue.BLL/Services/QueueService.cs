using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SmartQueue.BLL.IoT.CoffeeMachine;
using SmartQueue.Model.Entities;
using SmartQueue.Model.Repositories;
using SmartQueue.Model.Services;
using Order = SmartQueue.Model.Entities.Order;

namespace SmartQueue.BLL.Services
{
    class QueueService : IQueueService
    {
        private IUnitOfWork _unitOfWork;

        private static readonly object Locker= new object();

        private static readonly ConcurrentDictionary<long, List<Order>> Queue = new ConcurrentDictionary<long, List<Order>>();
        
        private readonly ICoffeeMachine _coffeeMachine;

        static QueueService()
        {
            Mapper.CreateMap<Order, IoT.CoffeeMachine.Order>()
                .ForMember(o => o.CoffeeMachineAddress, m => m.MapFrom(o => o.CoffeeMachine.Address));
        }

        public QueueService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _coffeeMachine = new CoffeeMachineClient();

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
            _coffeeMachine.MakeDrink(Mapper.Map<IoT.CoffeeMachine.Order>(order));
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
            return Queue[coffeeMachineId].OrderBy(c=>c.StartDate).Select(c => c.User).ToList();
        }

        public TimeSpan TimeLeft(long coffeeMachineId)
        {
            var result = _coffeeMachine.WaitFor(coffeeMachineId);
            return new TimeSpan(0, 0, result.SecondsToEnd);
        }

        public bool IsWait(long userId)
        {
            return Queue.SelectMany(q => q.Value).Any(o => o.UserId == userId);
        }

        public Order CurrentOrder(long userId)
        {
            return Queue.SelectMany(q => q.Value).FirstOrDefault(o => o.UserId == userId);
        }
    }
}
