using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartQueue.Model.Entities
{
    public class Company
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public User Director { get; set; }

        public ICollection<User> Employees { get; set; }

        public ICollection<CoffeeMachine> CoffeeMachines { get; set; } 
    }
}
