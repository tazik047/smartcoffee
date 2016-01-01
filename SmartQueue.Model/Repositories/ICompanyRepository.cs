﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartQueue.Model.Entities;

namespace SmartQueue.Model.Repositories
{
    public interface ICompanyRepository : IRepository<Company>
    {
        IEnumerable<Company> NotActiveCompanies();

        IEnumerable<Company> ActiveCompanies();
    }
}
