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
        private readonly IUnitOfWork _unitOfWork;

        //private static readonly object Locker= new object();

        //private static readonly ConcurrentDictionary<long, List<Order>> Queue = new ConcurrentDictionary<long, List<Order>>();
        
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
            /*lock (Locker)
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
            order.CoffeeMachine = _unitOfWork.CoffeeMachineRepository.Get(order.CoffeeMachineId);*/

            order.StartDate = DateTime.UtcNow;
            _unitOfWork.OrderRepository.Add(order); // CoffeeMachine???
            _unitOfWork.Save();
            order.CoffeeMachine = _unitOfWork.CoffeeMachineRepository.Get(order.CoffeeMachineId);
            _coffeeMachine.MakeDrink(Mapper.Map<IoT.CoffeeMachine.Order>(order));
        }

        public void RemoveFromQueue(long userId)
        {
            /*foreach (var item in Queue)
            {
                var order = item.Value.FirstOrDefault(o => o.UserId == userId);
                if (order != null)
                {
                    item.Value.Remove(order);
                    return;
                }
            }*/
            var order = _unitOfWork.OrderRepository.GetByUserId(userId);
            if (order == null)
            {
                return;
            }
            _unitOfWork.OrderRepository.Delete(order);
            _unitOfWork.Save();
        }

        public IEnumerable<User> GetAllFromQueue(long userId)
        {
            /*foreach (var item in Queue)
            {
                var order = item.Value.FirstOrDefault(o => o.UserId == userId);
                if (order != null)
                {
                    return Queue[order.CoffeeMachineId].OrderBy(c => c.StartDate).Select(c => c.User).ToList();
                }
            }
            return Enumerable.Empty<User>();*/
            var order = _unitOfWork.OrderRepository.GetByUserId(userId);
            if (order == null)
            {
                return Enumerable.Empty<User>();
            }
            return _unitOfWork.OrderRepository
                .Get(o => o.CoffeeMachineId == order.CoffeeMachineId)
                .OrderBy(o=>o.StartDate)
                .Select(o=>o.User)
                .ToList();
        }

        public TimeSpan TimeLeft(long userId)
        {
            /*foreach (var item in Queue)
            {
                var order = item.Value.FirstOrDefault(o => o.UserId == userId);
                if (order != null)
                {
                    var result = _coffeeMachine.WaitFor(order.CoffeeMachineId);
                    return new TimeSpan(0, 0, result.SecondsToEnd);
                }
            }
            return new TimeSpan(0);*/

            var order = _unitOfWork.OrderRepository.GetByUserId(userId);
            if (order == null)
            {
                return new TimeSpan(0);
            }
            var result = _coffeeMachine.WaitFor(order.CoffeeMachineId);
            if (result == null)
            {
                return new TimeSpan(0);
            }
            return new TimeSpan(0, 0, result.SecondsToEnd);
        }

        public bool IsWait(long userId)
        {
            return _unitOfWork.OrderRepository.Get().Any(o => o.UserId == userId);
        }

        public Order CurrentOrder(long userId)
        {
            return _unitOfWork.OrderRepository.GetByUserId(userId);
        }
    }
}
