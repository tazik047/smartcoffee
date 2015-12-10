using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartQueue.Model.Entities
{
    public class CoffeeMachine : IBaseEntity
    {
        public long Id { get; set; }

        public string Position { get; set; }

        public string Address { get; set; }

        public ICollection<Company> Companies { get; set; }

        public ICollection<User> ServiceStaff { get; set; }
    }
}
