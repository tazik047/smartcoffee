using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public IEnumerable<Company> NotActiveCompanies()
        {
            return DbSet.Include(c => c.Employees).Where(c => c.Employees.All(e => !e.IsActive)).ToList();
        } 
    }
}
