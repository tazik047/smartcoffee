using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using SmartQueue.Model.Entities;
using SmartQueue.Model.Services;
using SmartQueue.Web.Models;

namespace SmartQueue.Web.App_Start
{
    public static class AutoMapperConfig
    {
        private static ISmartQueueServices _smartQueueServices;

        public static void RegisterMaps()
        {
            _smartQueueServices = DependencyResolver.Current.GetService<ISmartQueueServices>();

            RegisterUserMaps();
            RegisterPreferencesMaps();
            RegisterCompanyMaps();
            RegisterCoffeeMachineMaps();
        }

        private static void RegisterCoffeeMachineMaps()
        {
            Mapper.CreateMap<CoffeeMachine, CoffeeMachineViewModel>()
                .ForMember(c => c.Company, m => m.MapFrom(c => c.Company.Name))
                .ForMember(c => c.ServiceStaff,
                    m => m.MapFrom( c =>
                                string.Join(", ",
                                    c.ServiceStaff
                                    .Select(s => string.Format("{0} {1}", s.Surname, s.Name))
                                    .ToList())
                                 )
                           );
            Mapper.CreateMap<CreateCoffeeMachineViewModel, CoffeeMachine>()
                .ForMember(c => c.Name, m => m.MapFrom(c => c.Address));
            Mapper.CreateMap<CoffeeMachine, EditCoffeeMachineViewModel>()
                .ForMember(c => c.ServiceStuff, m => m.MapFrom(c => c.ServiceStaff.Select(s => s.Id).ToList()));
            Mapper.CreateMap<EditCoffeeMachineViewModel, CoffeeMachine>()
                .ForMember(c => c.ServiceStaff,
                    m => m.MapFrom(c => _smartQueueServices.UserService.GetSelectedUsers(c.ServiceStuff)));
        }

        private static void RegisterCompanyMaps()
        {
            Mapper.CreateMap<Company, NotActiveCompanyViewModel>()
                .ForMember(c => c.Email,
                    m => m.MapFrom(c => string.Join(", ", c.Employees.Where(e => e.Roles.Any(r => r.Name.Equals("Director"))).Select(e=>e.Email).ToList())));
            Mapper.CreateMap<Company, AllCompaniesViewModel>()
                .ForMember(c => c.Email,
                    m =>
                        m.MapFrom(
                            c => string.Join(", ", c.Employees
                                        .Where(e => e.Roles.Any(r => r.Name.Equals("Director")))
                                        .Select(e => e.Email)
                                        .ToList())))
                .ForMember(c => c.AllEmployees, m => m.MapFrom(c => c.Employees.Count))
                .ForMember(c => c.ActivatedEmployees, m => m.MapFrom(c => c.Employees.Count(e => e.IsActive)));
        }

        private static void RegisterPreferencesMaps()
        {
            Mapper.CreateMap<CoffeePreferences, OrderViewModel>().ReverseMap();
            Mapper.CreateMap<Order, OrderViewModel>().ReverseMap();
        }

        private static void RegisterUserMaps()
        {
            Mapper.CreateMap<User, UserViewModel>().ReverseMap();
            Mapper.CreateMap<RegisterCompanyViewModel, User>()
                .ForMember(u => u.IsActive, m => m.UseValue(false))
                .ForMember(u => u.Company, m => m.MapFrom(u => new Company {Name = u.CompanyName}));
            Mapper.CreateMap<User, ManageUserViewModel>();
            Mapper.CreateMap<ManageUserViewModel, User>()
                .ForMember(u => u.Id, m => m.Ignore());
            Mapper.CreateMap<RegisterUserViewModel, User>();
            Mapper.CreateMap<User, UserQueueViewModel>();

        }
    }
}