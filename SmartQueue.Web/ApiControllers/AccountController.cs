using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using SmartQueue.Authorization.Infrastructure;
using SmartQueue.Authorization.Interfaces;
using SmartQueue.Model.Services;
using SmartQueue.Web.Models;

namespace SmartQueue.Web.ApiControllers
{
    public class AccountController : ApiController
    {
        private readonly ISmartQueueServices _smartQueueServices;

        private readonly IAuthorization _authorization;

        public AccountController(ISmartQueueServices smartQueueServices, IAuthorization authorization)
        {
            _smartQueueServices = smartQueueServices;
            _authorization = authorization;
        }

        public IHttpActionResult Get()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Ok(Mapper.Map<UserViewModel>(User.Identity.GetUser()));
            }

            return Unauthorized();
        }

        public IHttpActionResult Post([FromBody] LoginViewModel user)
        {
            if (user == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var loginedUser = _authorization.Login(user.Login, user.Password, user.RememberMe);
            if (loginedUser != null)
            {
                return Ok(Mapper.Map<UserViewModel>(loginedUser));
            }
            ModelState.AddModelError("", "Логин или пароль введен неверно.");
            return BadRequest(ModelState);
        }
    }
}
