using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using SmartQueue.Web.Models;

namespace SmartQueue.Web.ApiControllers
{
    public class DrinkTypesController : ApiController
    {
        public IHttpActionResult Get()
        {
            Func<object, string> getDisplayName = o =>
            {
                var result = null as string;
                var display = o.GetType()
                               .GetMember(o.ToString()).First()
                               .GetCustomAttributes(false)
                               .OfType<DisplayAttribute>()
                               .LastOrDefault();
                if (display != null)
                {
                    result = display.GetName();
                }

                return result ?? o.ToString();
            };

            var values = Enum.GetValues(typeof (DrinkTypeViewModel)).Cast<object>()
                             .Select(v => new 
                             {
                                 Text = getDisplayName(v),
                                 Value = (int)v
                             });
            return Ok(values);
        }
    }
}
