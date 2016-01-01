using System.ComponentModel.DataAnnotations;

namespace SmartQueue.Web.Models
{
    public class ManageUserViewModel
    {
        public long Id { get; set; }

        [Required]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}