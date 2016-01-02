using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace SmartCoffeeMachine
{
    public static class Util
    {
        public static async Task SendRequestAsync(long userId)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(WebConfigurationManager.AppSettings["appKey"]);
                var url = WebConfigurationManager.AppSettings["url"];
                var res = await httpClient.GetStringAsync(string.Format(url, userId));
            }
        }
    }
}