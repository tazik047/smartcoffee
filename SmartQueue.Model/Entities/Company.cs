﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartQueue.Model.Entities
{
    public class Company : IBaseEntity
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<User> Employees { get; set; }

        public virtual ICollection<CoffeeMachine> CoffeeMachines { get; set; } 
    }
}
