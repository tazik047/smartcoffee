using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartQueue.Web.Models
{
    public class CoffeeMachineViewModel
    {
        public long Id { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Расположение")]
        public string Position { get; set; }

        [Display(Name = "Адрес")]
        public string Address { get; set; }

        [Display(Name = "Компания")]
        public string Company { get; set; }

        [Display(Name = "Обслуживание")]
        public string ServiceStaff { get; set; }
    }
}