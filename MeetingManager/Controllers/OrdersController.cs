using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MeetingManager.Data;
using MeetingManager.Models;
using LibraryApi.Attributes;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace MeetingManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    public class OrdersController : ControllerBase
    {
        private readonly MeetingManagerContext _context;

        public OrdersController(MeetingManagerContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrder()
        {
            return await _context.Order.ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Order.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // GET: api/Orders/5
        [HttpGet]
        [Route("/api/Orders/GetOrdersByUserId/{id}")]
        public async Task<ActionResult<List<Order>>> GetOrdersByUserId(int id)
        {
            var orders = await _context.Order.Where(order => order.UserId == id).ToListAsync();

            return orders;
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(NewOrderModel newOrder)
        {
            Order order = new Order()
            {
                OfferId = newOrder.CartLineItem.OfferId,
                Comment = newOrder.Comment,
                From = newOrder.CartLineItem.From,
                To = newOrder.CartLineItem.To,
                Amount = newOrder.CartLineItem.TotalPrice,
                CreateDate = DateTime.Now,
                UserId = Int16.Parse(newOrder.UserId),
                Status = "Created"
            };

            _context.Order.Add(order);
            await _context.SaveChangesAsync();

            var user = await _context.User.FindAsync(order.UserId);

            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");
            var config = builder.Build();

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(config["Smtp:Host"]);

            mail.From = new MailAddress(config["Smtp:NotificationEmail"]);
            mail.To.Add(user.EmailAddress);
            mail.Subject = "MeetingManager | Your order was successfully created!";
            mail.Body = "We are happy, that you choose our service. Your order details:" +
                "Order no. #" + order.Id + "| Bill: " + order.Amount;

            SmtpServer.Port = Int16.Parse(config["Smtp:Port"]);
            SmtpServer.Credentials = new System.Net.NetworkCredential(config["Smtp:Username"], config["Smtp:Password"]);
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Order.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.Id == id);
        }
    }
}
