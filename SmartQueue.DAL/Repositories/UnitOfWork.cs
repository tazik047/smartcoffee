using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartQueue.Model.Repositories;

namespace SmartQueue.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EfDbContext _db;

        private ICoffeeMachineRepository _coffeeMachineRepository;

        private ICompanyRepository _companyRepository;

        private IRoleRepository _roleRepository;

        private IUserRepository _userRepository;

        private ICoffeePreferencesRepository _coffeePreferencesRepository;

        private IOrderRepository _orderRepository;

        public UnitOfWork()
        {
            _db = new EfDbContext();
        }

        public ICoffeeMachineRepository CoffeeMachineRepository
        {
            get { return _coffeeMachineRepository ?? (_coffeeMachineRepository = new CoffeeMachineRepository(_db)); }
        }

        public ICompanyRepository CompanyRepository
        {
            get { return _companyRepository ?? (_companyRepository = new CompanyRepository(_db)); }
        }

        public IRoleRepository RoleRepository
        {
            get { return _roleRepository ?? (_roleRepository = new RoleRepository(_db)); }
        }

        public IUserRepository UserRepository
        {
            get { return _userRepository ?? (_userRepository = new UserRepository(_db)); }
        }

        public ICoffeePreferencesRepository CoffeePreferencesRepository
        {
            get
            {
                return _coffeePreferencesRepository ??
                       (_coffeePreferencesRepository = new CoffeePreferencesRepository(_db));
            }
        }

        public IOrderRepository OrderRepository
        {
            get { return _orderRepository ?? (_orderRepository = new OrderRepository(_db)); }
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
