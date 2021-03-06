﻿using System.ComponentModel.DataAnnotations;

namespace SmartQueue.Web.Models
{
    public class UserViewModel
    {
        public long Id { get; set; }

        [Display(Name="Логин")]
        public string Login { get; set; }

        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Display(Name = "Фамилия")]
        public string Surname { get; set; }

        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Учетная запись активна?")]
        public bool IsActive { get; set; }
    }
}