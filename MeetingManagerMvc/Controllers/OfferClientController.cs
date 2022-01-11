using MeetingManager.Models;
using MeetingManagerMvc.Models;
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
    public class OfferClientController : Controller
    {
        private readonly HttpClient client;
        private readonly string WebApiPath;

        public OfferClientController(IConfiguration configuration)
        {
            var http = new HttpWebClient(configuration);
            client = http.GetClient();
            WebApiPath = http.GetWebApiPath();
        }

        public async Task<IActionResult> Index()
        {
            List<Offer> offers = null;
            HttpResponseMessage response = await client.GetAsync(WebApiPath + "Offers?PerPage=5");
            if (response.IsSuccessStatusCode)
            {
                offers = await response.Content.ReadAsAsync<List<Offer>>();
            }

            return View(offers);
        }

        public async Task<IActionResult> Details(int id)
        {

            HttpResponseMessage response = await client.GetAsync(WebApiPath + "Offers/" + id);
            if (response.IsSuccessStatusCode)
            {
                Offer offer = await response.Content.ReadAsAsync<Offer>();
                return View(offer);

            }

            return Redirect("/OfferClient");
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
                        return Redirect("/");
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
    }
}
