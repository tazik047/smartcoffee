using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using SmartQueue.Authorization.Infrastructure;
using SmartQueue.Model.Entities;
using SmartQueue.Model.Services;
using SmartQueue.Web.Models;

namespace SmartQueue.Web.ApiControllers
{
    public class QueueController : ApiController
    {
        private readonly ISmartQueueServices _smartQueueServices;

        public QueueController(ISmartQueueServices smartQueueServices)
        {
            _smartQueueServices = smartQueueServices;
        }

        public IHttpActionResult Get()
        {
            var userId = User.Identity.GetUser().Id;
            var users = _smartQueueServices.QueueService.GetAllFromQueue(userId);
            if (!users.Any())
            {
                return NotFound();
            }
            var result = new QueueViewModel
            {
                TimeToEnd = _smartQueueServices.QueueService.TimeLeft(userId),
                Users = Mapper.Map<IEnumerable<UserQueueViewModel>>(users)
            };
            return Ok(result);
        }

        public IHttpActionResult Post(OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var order = Mapper.Map<Order>(model);
                order.UserId = User.Identity.GetUser().Id;
                _smartQueueServices.QueueService.AddToQueue(order);
                return Ok();
            }
            return BadRequest(ModelState);
        }
    }
}
