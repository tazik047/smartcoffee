using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartQueue.Model.Entities
{
    public class User
    {
        public long Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public bool IsActive { get; set; }

        public CoffeeMachine FavouriteCoffeeMachine { get; set; }

        public Company Company { get; set; }

        public ICollection<Role> Roles { get; set; }
    }
}
