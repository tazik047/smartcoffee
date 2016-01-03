using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using SmartQueue.Authorization.Infrastructure;
using SmartQueue.Model.Services;
using SmartQueue.Web.Models;

namespace SmartQueue.Web.ApiControllers
{
    public class CoffeeMachineController : ApiController
    {
        private readonly ISmartQueueServices _smartQueueServices;

        public CoffeeMachineController(ISmartQueueServices smartQueueServices)
        {
            _smartQueueServices = smartQueueServices;
        }

        public IHttpActionResult Get()
        {
            var coffeeMachinies = _smartQueueServices.CoffeeMachineService.GetAllCoffeeMachines(User.Identity.GetUser().CompanyId.Value);
            return Ok(Mapper.Map<IEnumerable<CoffeeMachineViewModel>>(coffeeMachinies));
        }
    }
}
