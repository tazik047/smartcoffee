using System.ComponentModel.DataAnnotations;

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
