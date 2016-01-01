using System.ComponentModel.DataAnnotations;

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
