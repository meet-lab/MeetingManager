using MeetingManager.Models;
using MeetingManagerMvc.Models;
using MeetingManagerMvc.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MeetingManagerMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient client;
        private readonly string WebApiPath;

        public HomeController(IConfiguration configuration)
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

        public IActionResult Privacy()
        {
            return View();
        }

         async public Task<IActionResult> ThankYou(int id)
        {
            try
            {
                HttpResponseMessage orderResponse = await client.GetAsync(WebApiPath + "Orders/" + id);

                if (orderResponse.IsSuccessStatusCode)
                {
                    var order = await orderResponse.Content.ReadAsAsync<Order>();
                    
                    HttpResponseMessage userResponse = await client.GetAsync(WebApiPath + "UsersDetail/" + order.UserId);
                    var user = await userResponse.Content.ReadAsAsync<UserDetail>();

                    HttpResponseMessage OfferResponse = await client.GetAsync(WebApiPath + "Offers/" + order.OfferId);
                    var offert = await OfferResponse.Content.ReadAsAsync<Offer>();

                    userResponse.EnsureSuccessStatusCode();
                    OfferResponse.EnsureSuccessStatusCode();

                    ThankYouModel thankYou = new()
                    {
                        Order = order,
                        Offert = offert,
                        User = user
                    };

                    return View(thankYou);
                }

                return Redirect("/");
            }
            catch (Exception exception)
            {
                return Redirect("/Home/Error");
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
