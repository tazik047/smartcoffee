using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using SmartQueue.Model.Entities;

namespace SmartQueue.DAL
{
    class DbInitializer : DropCreateDatabaseAlways<EfDbContext>
    {
        protected override void Seed(EfDbContext context)
        {
            var roles = new[]
            {
                new Role {Name = "Administrator"},
                new Role {Name = "Director"},
                new Role {Name = "ServiceStaff"},
                new Role {Name = "User"},
            };
            foreach (var role in roles)
            {
                context.Roles.Add(role);
            }

            var director = new User {Login = "EpamLogin", Password = "Password", Email = "director@epam.com", Roles = new List<Role> {roles[1]}};

            context.Users.Add(director);

            var epam = new Company {Employees = new List<User>{director}, Name = "Epam"};

            context.Companies.Add(epam);

            var users = new[]
            {
                new User {Login = "Admin", Email = "admin@email.com", Password = "Password", Roles = new List<Role> {roles[0]}, IsActive = true},
                new User {Login = "ServiceStaff", Email = "service@email.com", Password = "Password", Roles = new List<Role> {roles[2]}, Company = epam},
                new User {Login = "Stas", Email = "user@email.com", Password = "Password", Roles = new List<Role> {roles[3]}, ContentType = "image/jpg", Company = epam},
                new User {Login = "Masha", Email = "user1@email.com", Password = "Password", Roles = new List<Role> {roles[3]}, Company = epam},
            };
            foreach (var user in users)
            {
                context.Users.Add(user);
            }

            base.Seed(context);
        }
    }
}
