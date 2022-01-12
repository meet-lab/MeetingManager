using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MeetingManager.Data;
using MeetingManager.Models;

namespace MeetingManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartLineItemsController : ControllerBase
    {
        private readonly MeetingManagerContext _context;

        public CartLineItemsController(MeetingManagerContext context)
        {
            _context = context;
        }

        // GET: api/CartLineItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartLineItem>>> GetCartLineItem()
        {
            return await _context.CartLineItem.ToListAsync();
        }

        // GET: api/CartLineItems/5
        [HttpGet("{cartId}")]
        public async Task<ActionResult<CartLineItem>> GetCartLineItemsByCartId(int cartId)
        {
            var cartLineItem = await _context.CartLineItem.Where(cart => cart.CartId == cartId).FirstOrDefaultAsync();

            if (cartLineItem == null)
            {
                return NotFound();
            }

            return cartLineItem;
        }

        // GET: api/CartLineItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CartLineItem>> GetCartLineItem(int id)
        {
            var cartLineItem = await _context.CartLineItem.FindAsync(id);

            if (cartLineItem == null)
            {
                return NotFound();
            }

            return cartLineItem;
        }

        // PUT: api/CartLineItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCartLineItem(int id, CartLineItem cartLineItem)
        {
            if (id != cartLineItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(cartLineItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartLineItemExists(id))
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

        // POST: api/CartLineItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CartLineItem>> PostCartLineItem(CartLineItem cartLineItem)
        {
            _context.CartLineItem.Add(cartLineItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCartLineItem", new { id = cartLineItem.Id }, cartLineItem);
        }

        // DELETE: api/CartLineItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartLineItem(int id)
        {
            var cartLineItem = await _context.CartLineItem.FindAsync(id);
            if (cartLineItem == null)
            {
                return NotFound();
            }

            _context.CartLineItem.Remove(cartLineItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CartLineItemExists(int id)
        {
            return _context.CartLineItem.Any(e => e.Id == id);
        }
    }
}
