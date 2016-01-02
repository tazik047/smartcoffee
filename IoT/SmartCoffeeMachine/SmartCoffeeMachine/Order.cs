using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SmartCoffeeMachine
{
    [DataContract]
    public class Order
    {
        [DataMember]
        public Size Size { get; set; }

        [DataMember]
        public int Sugar { get; set; }

        [DataMember]
        public DrinkType Drink { get; set; }

        [DataMember]
        public long CoffeeMachineId { get; set; }

        [DataMember]
        public string CoffeeMachineAddress { get; set; }

        [DataMember]
        public long UserId { get; set; }
    }
}
