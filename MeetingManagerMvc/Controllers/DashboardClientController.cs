using MeetingManager.Models;
using MeetingManagerMvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MeetingManagerMvc.Controllers
{
    public class DashboardClientController : Controller
    {
        private readonly HttpClient client;
        private readonly string WebApiPath;

        public DashboardClientController(IConfiguration configuration)
        {
            var http = new HttpWebClient(configuration);
            client = http.GetClient();
            WebApiPath = http.GetWebApiPath();
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var identity = User.Claims.Where(e => e.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault();

            if (identity == null)
            {
                return Redirect("/");
            }

            int userId = Int32.Parse(identity.Value);
            List<Offer> offers = null;
            HttpResponseMessage response = await client.GetAsync(WebApiPath + $"Offers?userId={userId}");
            if (response.IsSuccessStatusCode)
            {
                offers = await response.Content.ReadAsAsync<List<Offer>>();
            }

            return View(offers);
        }
    }
}
