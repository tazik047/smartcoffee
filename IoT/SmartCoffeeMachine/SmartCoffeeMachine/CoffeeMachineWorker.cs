using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace SmartCoffeeMachine
{
    class CoffeeMachineWorker
    {
        private static CoffeeMachineWorker _instance;

        private readonly object _locker = new object();
        private ConcurrentDictionary<long, ConcurrentQueue<QueueItem>> _queue;

        private CoffeeMachineWorker()
        {
            _queue = new ConcurrentDictionary<long, ConcurrentQueue<QueueItem>>();
            Task.Run(() => Worker());
        }

        private void Worker()
        {
            while (true)
            {
                lock (_locker)
                {
                    foreach (var item in _queue)
                    {
                        if (item.Value.Count > 0)
                        {
                            QueueItem queueItem;
                            item.Value.TryPeek(out queueItem);
                            queueItem.SecondsToEnd--;
                            if (queueItem.SecondsToEnd == 0)
                            {
                                item.Value.TryDequeue(out queueItem);
                                Task.Run(() => Util.SendRequestAsync(queueItem.UserId));
                            }
                        }
                    }
                }
                Thread.Sleep(1000);
            }
        }

        public QueueItem TimeForEnd(long coffeeMachine)
        {
            QueueItem queueItem;
            lock (_locker)
            {
                _queue[coffeeMachine].TryPeek(out queueItem);
            }
            return queueItem;
        }

        public void AddToQueue(Order order)
        {
            int timeToEnd = 40;
            switch (order.Drink)
            {
                case DrinkType.Coffee:
                    timeToEnd = 50;
                    break;
                case DrinkType.Tea:
                    timeToEnd = 40;
                    break;
            }
            switch (order.Size)
            {
                case Size.Medium:
                    timeToEnd += 10;
                    break;
                case Size.Large:
                    timeToEnd += 20;
                    break;
            }
            lock (_locker)
            {
                if (!_queue.ContainsKey(order.CoffeeMachineId))
                {
                    _queue[order.CoffeeMachineId] = new ConcurrentQueue<QueueItem>();
                }
                _queue[order.CoffeeMachineId].Enqueue(new QueueItem { SecondsToEnd = timeToEnd, UserId = order.UserId });
            }
        }

        public static CoffeeMachineWorker GetInstance()
        {
            return _instance ?? (_instance = new CoffeeMachineWorker());
        }
    }
}