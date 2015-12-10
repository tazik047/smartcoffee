using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartQueue.Model.Entities;
using SmartQueue.Model.Repositories;

namespace SmartQueue.DAL.Repositories
{
    class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        public CompanyRepository(EfDbContext context) : base(context)
        {
        }
    }
}
