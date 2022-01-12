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
    [Authorize]
    public class CartClientController : Controller
    {
        private readonly HttpClient client;
        private readonly string WebApiPath;

        public CartClientController(IConfiguration configuration)
        {
            var http = new HttpWebClient(configuration);
            client = http.GetClient();
            WebApiPath = http.GetWebApiPath();
        }

        [HttpGet]
        public async Task<IActionResult> Confirm(int offerId)
        {
            // Nie mogę pobrać id z request
            HttpResponseMessage offertResponse = await client.GetAsync(WebApiPath + "Offers/" + 1);
            Offer offert = await offertResponse.Content.ReadAsAsync<Offer>();

            LineItemModel lineItemModel = new()
            {
                Offer = offert,
                From = DateTime.Today,
                To = DateTime.Today.AddDays(1)
            };

            return View(lineItemModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int offerId, [Bind("From,To")] LineItemModel lineItem)
        {
            var identity = User.Claims.Where(e => e.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault();

            if (identity == null)
            {
                return Redirect("/");
            }
                
            try
            {
                HttpResponseMessage cartResponse = await client.GetAsync(WebApiPath + "Carts/" + identity.Value);
                Cart cart = await cartResponse.Content.ReadAsAsync<Cart>();

                HttpResponseMessage offertResponse = await client.GetAsync(WebApiPath + "Offers/" + offerId);
                Offer offert = await offertResponse.Content.ReadAsAsync<Offer>();

                if (cartResponse.IsSuccessStatusCode)
                {
                    lineItem.CartId = cart.Id;
                    // Jak dodam offerte jako obiekt tworzy się błąd 500
                    HttpResponseMessage cartLineItemResponse = await client.PostAsJsonAsync(WebApiPath + "CartLineItems/", lineItem);

                    return Redirect("/OfferClient/Details/" + offert.Id);
                }
            }
            catch (Exception exception)
            {
                return Redirect("/Home/Error");
            }

            return Redirect("/");
        }

        public async Task<IActionResult> Index()
        {
            var identity = User.Claims.Where(e => e.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault();

            if (identity == null)
            {
                return Redirect("/");
            }

            HttpResponseMessage cartResponse = await client.GetAsync(WebApiPath + "Carts/" + identity.Value);
            Cart cart = await cartResponse.Content.ReadAsAsync<Cart>();

            List<CartLineItem> lineItems = null;
            HttpResponseMessage response = await client.GetAsync(WebApiPath + "CartLineItems/?cartId" + cart.Id);
            if (response.IsSuccessStatusCode)
            {
                lineItems = await response.Content.ReadAsAsync<List<CartLineItem>>();
            }

            return View(lineItems);
        }


        // GET: CartLineItems/Delete/5
        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            HttpResponseMessage response = await client.GetAsync(WebApiPath + "CartLineItems/GetLineItem/" + id);
            
            if (response.IsSuccessStatusCode)
            {
                CartLineItem lineItem = await response.Content.ReadAsAsync<CartLineItem>();
                return View(lineItem);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, int notUsed = 0)
        {
            try
            {
                HttpResponseMessage response = await client.DeleteAsync(WebApiPath + "CartLineItems/" + id);
                response.EnsureSuccessStatusCode();

                return Redirect("/CartClient/Index");
            }
            catch (Exception exception)
            {
                return Redirect("/Home/Error");
            }
        }
    }
}
