using System.ComponentModel.DataAnnotations;

namespace SmartQueue.Web.Models
{
    public class AllCompaniesViewModel
    {
        public long Id { get; set; }

        [Display(Name = "Название компании")]
        public string Name { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "К-во всех сотрудников")]
        public long AllEmployees { get; set; }

        [Display(Name = "К-во активных сотрудников")]
        public long ActivatedEmployees { get; set; }
    }
}