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
            HttpResponseMessage response = await client.GetAsync(WebApiPath + $"Offers/OwnerOffers/{userId}&offerStatus={status}");
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
            HttpResponseMessage response = await client.GetAsync(WebApiPath + $"Offers/OwnerOffers/{userId}");
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
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Id,Title,Description,Price,Status")] Offer offer)
        {
            var identity = User.Claims.Where(e => e.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault();

            if (identity == null)
            {
                return Redirect("/");
            }
            int userId = Int32.Parse(identity.Value);
            HttpResponseMessage response = await client.PutAsJsonAsync(WebApiPath + "Offers/" + id, offer);
            if (response.IsSuccessStatusCode)
            {
                return Redirect("/DashboardClient");
            }

            return Redirect("/DashboardClient");
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(MeetingManagerMvc.Models.CreateOfferModel model)
        {
            var identity = User.Claims.Where(e => e.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault();

            if (identity == null)
            {
                return Redirect("/");
            }

            if (ModelState.IsValid)
            {
                int userId = Int32.Parse(identity.Value);

                model.UserId = userId;
                HttpResponseMessage response = await client.PostAsJsonAsync(WebApiPath + "Offers", model);

                try
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Message = "Offer has been created!";
                        return LocalRedirect("/");
                    }
                    else
                    {
                        ViewBag.Message = "Provided values is not valid.";
                        return View(model);
                    }
                }
                catch (Exception exception)
                {
                    return Redirect("/Home/Error");
                }
            }
            return View(model);
        }

        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            HttpResponseMessage response = await client.GetAsync(WebApiPath + "Offers/" + id);

            if (response.IsSuccessStatusCode)
            {
                Offer offer = await response.Content.ReadAsAsync<Offer>();
                return View(offer);

            }

            return LocalRedirect("/");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Delete(int id, [Bind("")] Offer offer)
        {
            if (ModelState.IsValid)
            {
                
                    HttpResponseMessage response = await client.DeleteAsync(WebApiPath + "Offers/" + id);
                    response.EnsureSuccessStatusCode();

                    return LocalRedirect("/");
            }

            return View(offer);
        }
    }
}
