using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartQueue.Model.Entities
{
    public class Order : IBaseEntity
    {
        public long Id { get; set; }

        public Size Size { get; set; }

        public int Sugar { get; set; }

        public DrinkType Drink { get; set; }

        public long CoffeeMachineId { get; set; }

        public virtual CoffeeMachine CoffeeMachine { get; set; }

        public long UserId { get; set; }

        public virtual User User { get; set; }

        public DateTime StartDate { get; set; }
    }
}
