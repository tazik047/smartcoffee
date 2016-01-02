using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SmartCoffeeMachine
{
    [DataContract]
    public class QueueItem
    {
        [DataMember]
        public long UserId { get; set; }

        [DataMember]
        public int SecondsToEnd { get; set; }
    }
}