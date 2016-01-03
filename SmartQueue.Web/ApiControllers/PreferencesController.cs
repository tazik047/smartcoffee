using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using AutoMapper;
using SmartQueue.Authorization.Infrastructure;
using SmartQueue.Model.Entities;
using SmartQueue.Model.Services;
using SmartQueue.Web.Models;

namespace SmartQueue.Web.ApiControllers
{
    public class PreferencesController : ApiController
    {
        private readonly ISmartQueueServices _smartQueueServices;

        public PreferencesController(ISmartQueueServices smartQueueServices)
        {
            _smartQueueServices = smartQueueServices;
        }

        public IHttpActionResult Get()
        {
            var user = User.Identity.GetUser();
            var preferences = _smartQueueServices.PreferencesService.GetUserPreferences(user);
            var model = Mapper.Map<OrderViewModel>(preferences);
            return Ok(model);
        }

        public IHttpActionResult Post([FromBody]OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = User.Identity.GetUser();
                _smartQueueServices.PreferencesService
                    .UpdateUserPreferences(user, Mapper.Map<CoffeePreferences>(model));
                return Ok();
            }

            return BadRequest(ModelState);
        }
    }
}
