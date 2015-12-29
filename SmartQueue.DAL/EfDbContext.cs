using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartQueue.Model.Entities;

namespace SmartQueue.DAL
{
    class EfDbContext : DbContext
    {
        static EfDbContext()
        {
            Database.SetInitializer(new DbInitializer());
        }

        public EfDbContext()
            : base("coffeeContext")
        {
        }

        public DbSet<CoffeeMachine> CoffeeMachines { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<CoffeePreferences> CoffeePreferenceses { get; set; }
        
        public virtual void SetState(object item, EntityState state)
        {
            Entry(item).State = state;
        }
    }
}
