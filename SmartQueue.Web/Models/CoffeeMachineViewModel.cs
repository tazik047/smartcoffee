using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartQueue.Web.Models
{
    public class CoffeeMachineViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Position { get; set; }

        public string Address { get; set; }

        public string Company { get; set; }

        public string ServiceStaff { get; set; }
    }
}