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

            var users = new[]
            {
                new User {Login = "Admin", Email = "admin@email.com", Password = "Password", Roles = new List<Role> {roles[0]}},
                new User {Login = "Director", Email = "director@email.com", Password = "Password", Roles = new List<Role> {roles[1]}},
                new User {Login = "ServiceStaff", Email = "service@email.com", Password = "Password", Roles = new List<Role> {roles[2]}},
                new User {Login = "Stas", Email = "user@email.com", Password = "Password", Roles = new List<Role> {roles[3]}},
                new User {Login = "Masha", Email = "user1@email.com", Password = "Password", Roles = new List<Role> {roles[3]}},
            };
            foreach (var user in users)
            {
                context.Users.Add(user);
            }

            base.Seed(context);
            base.Seed(context);
        }
    }
}
