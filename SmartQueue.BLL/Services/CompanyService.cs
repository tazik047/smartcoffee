using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartQueue.Model.Entities;
using SmartQueue.Model.Repositories;
using SmartQueue.Model.Services;

namespace SmartQueue.BLL.Services
{
    class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void ActivateCompany(long companyId)
        {
            var directors =
                _unitOfWork.CompanyRepository
                    .Get(companyId).Employees
                    .Where(e => !e.IsActive && e.Roles.Any(r => r.Name.Equals("Director")))
                    .ToList();

            foreach (var director in directors)
            {
                director.IsActive = true;
                _unitOfWork.UserRepository.Edit(director);
            }

            _unitOfWork.Save();
        }

        public IEnumerable<Company> GetNotActiveCompany()
        {
            return _unitOfWork.CompanyRepository.NotActiveCompanies();
        }

        public void BanCompany(long companyId)
        {
            SetActiveStatus(false, companyId);
        }

        public void ActivateAllEmployeesCompany(long companyId)
        {
            SetActiveStatus(true, companyId);
        }

        public IEnumerable<Company> GetAllCompanies()
        {
            return _unitOfWork.CompanyRepository.ActiveCompanies();
        }

        public IEnumerable<User> GetAllEmployees(long companyId)
        {
            return _unitOfWork.UserRepository.Get(u => u.CompanyId == companyId).ToList();
        }

        public IEnumerable<User> GetAllNotActiveEmployees(long companyId)
        {
            return _unitOfWork.UserRepository.Get(u => u.CompanyId == companyId && !u.IsActive).ToList();
        }

        private void SetActiveStatus(bool activeStatus, long companyId)
        {
            foreach (var employee in _unitOfWork.UserRepository.Get(u => u.CompanyId == companyId).ToList())
            {
                employee.IsActive = activeStatus;
                _unitOfWork.UserRepository.Edit(employee);
            }
            _unitOfWork.Save();
        }
    }
}
