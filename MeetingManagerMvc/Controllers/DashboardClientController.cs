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
        public async Task<IActionResult> Index([FromQuery] string status)
        {
            var identity = User.Claims.Where(e => e.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault();

            if (identity == null)
            {
                return Redirect("/");
            }

            int userId = Int32.Parse(identity.Value);
            List<Offer> offers = null;
            HttpResponseMessage response = await client.GetAsync(WebApiPath + $"Offers?userId={userId}&offerStatus={status}");
            if (response.IsSuccessStatusCode)
            {
                offers = await response.Content.ReadAsAsync<List<Offer>>();
            }

            return View(offers);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var identity = User.Claims.Where(e => e.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault();

            if (identity == null)
            {
                return Redirect("/");
            }
            int userId = Int32.Parse(identity.Value);
            HttpResponseMessage response = await client.GetAsync(WebApiPath + "Offers/" + id + $"?userId={userId}");
            if (response.IsSuccessStatusCode)
            {
                Offer offer = await response.Content.ReadAsAsync<Offer>();
                return View(offer);

            }

            return Redirect("/DashboardClient");
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Title,Description,Price,Status")] Offer offer)
        {
            var identity = User.Claims.Where(e => e.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault();

            if (identity == null)
            {
                return Redirect("/");
            }
            int userId = Int32.Parse(identity.Value);
            HttpResponseMessage response = await client.PutAsJsonAsync(WebApiPath + "Offers/" + id + $"?userId={userId}", offer);
            if (response.IsSuccessStatusCode)
            {
                return Redirect("/DashboardClient");
            }

            return Redirect("/DashboardClient");
        }

        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            var identity = User.Claims.Where(e => e.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault();

            if (identity == null)
            {
                return Redirect("/");
            }
            int userId = Int32.Parse(identity.Value);
            HttpResponseMessage response = await client.GetAsync(WebApiPath + "Offers/" + id + $"?userId={userId}");

            if (response.IsSuccessStatusCode)
            {
                Offer offer = await response.Content.ReadAsAsync<Offer>();
                return View(offer);

            }

            return Redirect("/DashboardClient");
        }
    }
}
