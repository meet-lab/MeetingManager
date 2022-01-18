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
    public class OrderClientController : Controller
    {
        private readonly HttpClient client;
        private readonly string WebApiPath;

        public OrderClientController(IConfiguration configuration)
        {
            var http = new HttpWebClient(configuration);
            client = http.GetClient();
            WebApiPath = http.GetWebApiPath();
        }

        [HttpGet]
        public async Task<IActionResult> Create(int id)
        {
            HttpResponseMessage response = await client.GetAsync(WebApiPath + "CartLineItems/GetCartLineItem/" + id);
            CartLineItem cartLineItem = await response.Content.ReadAsAsync<CartLineItem>();

            HttpResponseMessage offertResponse = await client.GetAsync(WebApiPath + "Offers/" + cartLineItem.OfferId);
            Offer offert = await offertResponse.Content.ReadAsAsync<Offer>();

            NewOrderModel newOrder = new()
            {
                Offer = offert,
                CartLineItem = cartLineItem,
                Comment = ""
            };

            return View(newOrder);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int id, [Bind("Offert,Comment,CartLineItem")] NewOrderModel newOrderModel)
        {
            var identity = User.Claims.Where(e => e.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault();

            if (identity == null)
            {
                return Redirect("/");
            }

            try
            {
                HttpResponseMessage response = await client.GetAsync(WebApiPath + "CartLineItems/GetCartLineItem/" + id);
                CartLineItem cartLineItem = await response.Content.ReadAsAsync<CartLineItem>();

                HttpResponseMessage offertResponse = await client.GetAsync(WebApiPath + "Offers/" + cartLineItem.OfferId);
                offertResponse.EnsureSuccessStatusCode();

                Offer offert = await offertResponse.Content.ReadAsAsync<Offer>();

                newOrderModel.Offer = offert;
                newOrderModel.CartLineItem = cartLineItem;
                newOrderModel.UserId = identity.Value;

                HttpResponseMessage orderResponse = await client.PostAsJsonAsync(WebApiPath + "Orders/", newOrderModel);
                orderResponse.EnsureSuccessStatusCode();

                HttpResponseMessage deleteLineItemReponse = await client.DeleteAsync(WebApiPath + "CartLineItems/" + cartLineItem.Id);
                var order = await orderResponse.Content.ReadAsAsync<Order>();

                return Redirect("/Home/ThankYou/" + order.Id);
            }
            catch (Exception exception)
            {
                return Redirect("/Home/Error");
            }

            return Redirect("/");
        }


        public async Task<IActionResult> List([FromQuery] string orderStatus)
        {
            var identity = User.Claims.Where(e => e.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault();

            if (identity == null)
            {
                return Redirect("/");
            }

            HttpResponseMessage response = await client.GetAsync(WebApiPath + "Orders/GetOrdersByUserId/" + Int16.Parse(identity.Value) + "?orderStatus="+orderStatus);

            List<Order> orders = null;
            if (response.IsSuccessStatusCode)
            {
                orders = await response.Content.ReadAsAsync<List<Order>>();
            }

            return View(orders);
        }

        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(WebApiPath + "Orders/" + id);

                if (response.IsSuccessStatusCode)
                {
                    Order order = await response.Content.ReadAsAsync<Order>();
                    order.Status = "Canceled";
                    order.EditDate = DateTime.Now;

                    HttpResponseMessage editResponse = await client.PutAsJsonAsync(WebApiPath + "Orders/" + id, order);
                    response.EnsureSuccessStatusCode();

                    return Redirect("/OrderClient/List");
                }

                return NotFound();
            }
            catch (Exception exception)
            {
                return Redirect("/Home/Error");
            }
        }

        public async Task<ActionResult> OrderDetail(int id)
        {
            try
            {
                HttpResponseMessage orderResponse = await client.GetAsync(WebApiPath + "Orders/" + id);

                if (orderResponse.IsSuccessStatusCode)
                {
                    var order = await orderResponse.Content.ReadAsAsync<Order>();

                    HttpResponseMessage offertResponse = await client.GetAsync(WebApiPath + "Offers/" + order.OfferId);
                    offertResponse.EnsureSuccessStatusCode();

                    var offert = await offertResponse.Content.ReadAsAsync<Offer>();

                    OrderDetailModel orderDetail = new()
                    {
                        Order = order,
                        Offert = offert
                    };

                    return View(orderDetail);
                }

                return Redirect("/Home/Error");
            }
            catch (Exception exception)
            {
                return Redirect("/Home/Error");
            }
        }
    }
}
