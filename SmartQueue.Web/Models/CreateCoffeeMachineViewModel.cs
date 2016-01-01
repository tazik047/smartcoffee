using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartQueue.Web.Models
{
    public class CreateCoffeeMachineViewModel
    {
        [Required]
        [Display(Name = "IP кофеварки")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Фирма-владелец")]
        public long CompanyId { get; set; }

        public List<SelectListItem> Companies { get; set; } 
    }
}