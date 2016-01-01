using System.ComponentModel.DataAnnotations;

namespace SmartQueue.Web.Models
{
    public class ChangePasswordViewModel
    {
        [Required]
        [MinLength(6)]
        [MaxLength(30)]
        [Display(Name = "Старый пароль")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(30)]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        [Display(Name = "Повторите пароль")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}