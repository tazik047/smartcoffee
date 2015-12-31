﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartQueue.Model.Entities;

namespace SmartQueue.Model.Services
{
    public interface ICompanyService
    {
        void ActivateCompany(long companyId);

        IEnumerable<Company> GetNotActiveCompany();

        void BanCompany(long companyId);

        void ActivateAllEmployeesCompany(long companyId);
    }
}
