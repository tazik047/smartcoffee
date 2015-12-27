using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartQueue.Web.Models
{
    public class UserViewModel
    {
        public long Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }
    }
}