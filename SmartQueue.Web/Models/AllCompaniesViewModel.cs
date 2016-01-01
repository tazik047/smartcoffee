using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartQueue.Web.Models
{
    public class AllCompaniesViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public long AllEmployees { get; set; }

        public long ActivatedEmployees { get; set; }
    }
}