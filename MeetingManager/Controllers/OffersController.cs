using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MeetingManager.Data;
using MeetingManager.Models;

namespace MeetingManager
{
    [Route("api/[controller]")]
    [ApiController]
    public class OffersController : ControllerBase
    {
        private readonly MeetingManagerContext _context;

        public OffersController(MeetingManagerContext context)
        {
            _context = context;
        }

        // GET: api/Offers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Offer>>> GetOffer(string perPage, string userId)
        {
           List<Offer> offers = null;
            //   int parserPerPage = Int32.Parse(PerPage);

            //   if (PerPage != null && parserPerPage != 0)
            //   {
            //       var takenOffers = offers.Take(4);
            //       return takenOffers;
            //   }

            offers = await _context.Offer.ToListAsync();
            if (userId != null)
            {
                int parsedUserId;
                var parsingSuccess = int.TryParse(userId, out parsedUserId);
                if(parsingSuccess)
                {
                    return await _context.Offer.Where(o => o.UserId == parsedUserId).ToListAsync();
                }
                return offers;
            }


            return offers;
        }

        // GET: api/Offers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Offer>> GetOffer(int id)
        {
            var offer = await _context.Offer.FindAsync(id);

            if (offer == null)
            {
                return NotFound();
            }

            return offer;
        }

        // PUT: api/Offers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOffer(int id, Offer offer)
        {
            if (id != offer.Id)
            {
                return BadRequest();
            }

            offer.UpdatedAt = DateTime.Now;

            _context.Entry(offer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OfferExists(id))
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

        // POST: api/Offers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Offer>> PostOffer(CreateOfferModel offer)
        {
                
                Offer newOffer = new Offer
                {
                    Title = offer.Title,
                    Description = offer.Description,
                    Status = offer.Status,
                    Price = offer.Price,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    UserId = offer.UserId,
                };

            _context.Offer.Add(newOffer);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOffer", new { id = newOffer.Id }, newOffer);
        }

        // DELETE: api/Offers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOffer(int id)
        {
            var offer = await _context.Offer.FindAsync(id);
            if (offer == null)
            {
                return NotFound();
            }

            _context.Offer.Remove(offer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OfferExists(int id)
        {
            return _context.Offer.Any(e => e.Id == id);
        }
    }
}
