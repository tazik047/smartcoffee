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
        }

        private static void RegisterCompanyMaps()
        {
            Mapper.CreateMap<Company, NotActiveCompanyViewModel>()
                .ForMember(c => c.Email,
                    m => m.MapFrom(c => string.Join(", ", c.Employees.Where(e => e.Roles.Any(r => r.Name.Equals("Director"))).Select(e=>e.Email).ToList())));
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
        }
    }
}