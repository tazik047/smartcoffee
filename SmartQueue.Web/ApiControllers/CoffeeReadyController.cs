using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;
using SmartQueue.Model.Services;

namespace SmartQueue.Web.ApiControllers
{
    public class CoffeeReadyController : ApiController
    {
        private readonly ISmartQueueServices _smartQueueServices;

        public CoffeeReadyController(ISmartQueueServices smartQueueServices)
        {
            _smartQueueServices = smartQueueServices;
        }

        IHttpActionResult Get(long id)
        {
            if (Request.Headers.Authorization.Parameter != WebConfigurationManager.AppSettings["appKey"])
            {
                return Unauthorized();
            }
            _smartQueueServices.QueueService.RemoveFromQueue(id);
            return Ok();
        }
    }
}
