using System.ComponentModel.DataAnnotations;

namespace SmartQueue.Web.Models
{
    public class OrderViewModel
    {
        public long Id { get; set; }

        [Required]
        [UIHint("Enum")]
        [Display(Name = "Порция")]
        public SizeViewModel Size { get; set; }

        [Required]
        [Range(0, 10)]
        [Display(Name = "Ложек сахара")]
        public int Sugar { get; set; }

        [Required]
        [UIHint("Enum")]
        [Display(Name = "Напиток")]
        public DrinkTypeViewModel Drink { get; set; }
    }
}