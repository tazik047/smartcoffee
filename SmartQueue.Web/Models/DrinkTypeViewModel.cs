using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartQueue.Web.Models
{
    public enum DrinkTypeViewModel
    {
        [Display(Name = "Кофе")]
        Coffee,
        [Display(Name = "Чай")]
        Tea
    }
}
