using MeetingManagerMvc.Models;
using MeetingManagerMvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
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

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateOfferModel model)
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
