using System.ComponentModel.DataAnnotations;

namespace SmartQueue.Web.Models
{
    public class NotActiveCompanyViewModel
    {
        public long Id { get; set; }

        [Display(Name="Название компании")]
        public string Name { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}