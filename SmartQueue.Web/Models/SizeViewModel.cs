using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartQueue.Web.Models
{
    public enum SizeViewModel
    {
        [Display(Name = "Маленькая")]
        Smalle,
        [Display(Name = "Средняя")]
        Medium,
        [Display(Name = "Большая")]
        Large
    }
}
