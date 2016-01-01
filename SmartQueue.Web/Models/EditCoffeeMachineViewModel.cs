using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartQueue.Web.Models
{
    public class EditCoffeeMachineViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Position { get; set; }

        public List<long> ServiceStuff { get; set; }

        public List<SelectListItem> Employees { get; set; }
    }
}