using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using SmartQueue.Model.Entities;
using SmartQueue.Web.Models;

namespace SmartQueue.Web.App_Start
{
    public static class AutoMapperConfig
    {
        public static void RegisterMaps()
        {
            RegisterUserMaps();
        }

        private static void RegisterUserMaps()
        {
            Mapper.CreateMap<User, UserViewModel>().ReverseMap();
        }
    }
}