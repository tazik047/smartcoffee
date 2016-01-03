using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using SmartQueue.Model.Entities;
using SmartQueue.Utils.Sequrity;

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
                new Role {Name = "User"},
            };
            foreach (var role in roles)
            {
                context.Roles.Add(role);
            }

            var password = Encrypter.HashText("Password");

            var director = new User
            {
                Login = "EpamLogin",
                Password = password,
                Surname = "Директор",
                Name = "Директор",
                Email = "director@epam.com",
                Roles = new List<Role> {roles[1]},
                IsActive = true
            };

            context.Users.Add(director);

            var epam = new Company {Employees = new List<User>{director}, Name = "Epam"};

            context.Companies.Add(epam);

            var users = new[]
            {
                new User {Login = "Admin", Email = "admin@email.com", Password = password, Roles = new List<Role> {roles[0]}, IsActive = true, ContentType = "iamge/gif", Name = "Admin", Surname = "Admin"},
                new User {Login = "Stas", Email = "user@email.com", Password = password, Roles = new List<Role> {roles[2]}, ContentType = "image/jpg", Company = epam, Name = "Stas", Surname = "Stas", IsActive = true},
                new User {Login = "Masha", Email = "user1@email.com", Password = password, Roles = new List<Role> {roles[2]}, Company = epam, Name = "Masha", Surname = "Masha"},
            };
            foreach (var user in users)
            {
                context.Users.Add(user);
            }

            var coffeeMachine = new CoffeeMachine
            {
                Address = "127.0.0.1:7878",
                Company = epam,
                Name = "Coffee",
                Position = "1 floor",
                ServiceStaff = new List<User> {director},
            };

            context.CoffeeMachines.Add(coffeeMachine);

            context.SaveChanges();

            base.Seed(context);
        }
    }
}
