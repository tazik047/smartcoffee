using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartQueue.Web.Models
{
    public class EditCoffeeMachineViewModel
    {
        public long Id { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Расположение")]
        public string Position { get; set; }

        [Display(Name = "Обслуживающий персонал")]
        public List<long> ServiceStuff { get; set; }

        public List<SelectListItem> Employees { get; set; }
    }
}