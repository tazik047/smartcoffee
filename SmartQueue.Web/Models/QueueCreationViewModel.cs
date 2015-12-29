using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartQueue.Web.Models
{
    public class QueueCreationViewModel
    {
        public long CoffeeMachineId { get; set; }

        public OrderViewModel Order { get; set; }


    }
}