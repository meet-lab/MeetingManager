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
        public async Task<IActionResult> Confirm(int id)
        {
            HttpResponseMessage offertResponse = await client.GetAsync(WebApiPath + "Offers/" + id);
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
                    lineItem.Price = offert.Price;
                    lineItem.Name = offert.Title;
                    lineItem.CartId = cart.Id;
                    lineItem.OfferId = offert.Id;

                    HttpResponseMessage cartLineItemResponse = await client.PostAsJsonAsync(WebApiPath + "CartLineItems/", lineItem);
                    cartLineItemResponse.EnsureSuccessStatusCode();

                    return Redirect("/CartClient/Index/");
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
            HttpResponseMessage response = await client.GetAsync(WebApiPath + "CartLineItems/GetByCardId/" + cart.Id);

            lineItems = await response.Content.ReadAsAsync<List<CartLineItem>>();

            return View(lineItems);
        }

        public async Task<ActionResult> Delete(int id)
        {
            HttpResponseMessage response = await client.GetAsync(WebApiPath + "CartLineItems/GetCartLineItem/" + id);
            
            if (response.IsSuccessStatusCode)
            {

                try
                {
                    CartLineItem lineItem = await response.Content.ReadAsAsync<CartLineItem>();

                    HttpResponseMessage deelteRespone = await client.DeleteAsync(WebApiPath + "CartLineItems/" + lineItem.Id);
                    response.EnsureSuccessStatusCode();

                    return Redirect("/CartClient/Index");
                }
                catch (Exception exception)
                {
                    return Redirect("/Home/Error");
                }
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

        public async Task<ActionResult> Edit(int id)
        {
            HttpResponseMessage response = await client.GetAsync(WebApiPath + "CartLineItems/GetCartLineItem/" + id);
            if (response.IsSuccessStatusCode)
            {
                CartLineItem cartLineItem = await response.Content.ReadAsAsync<CartLineItem>();
                return View(cartLineItem);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, CartLineItem cartLineItem)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage offertResponse = await client.GetAsync(WebApiPath + "Offers/" + cartLineItem.OfferId);
                Offer offert = await offertResponse.Content.ReadAsAsync<Offer>();

                cartLineItem.TotalPrice = (decimal)(((cartLineItem.To - cartLineItem.From).Days) * offert.Price);

                HttpResponseMessage response = await client.PutAsJsonAsync(WebApiPath + "CartLineItems/" + cartLineItem.Id, cartLineItem);
                response.EnsureSuccessStatusCode();

                return RedirectToAction(nameof(Index));
            }

            return View(cartLineItem);
        }
    }
}
