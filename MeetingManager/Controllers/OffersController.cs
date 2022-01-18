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

namespace MeetingManager
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    public class OffersController : ControllerBase
    {
        private readonly MeetingManagerContext _context;

        public OffersController(MeetingManagerContext context)
        {
            _context = context;
        }

        // GET: api/Offers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Offer>>> GetOffer([FromQuery] string perPage, [FromQuery] string minPrice, [FromQuery] string maxPrice, [FromQuery] string offerTitle)
        {
            try
            {
                int parsedPerPerNum;
                int minPriceParsed;
                int maxPriceParsed;
                var parsingSuccess = int.TryParse(perPage, out parsedPerPerNum);

                if (perPage != null && parsingSuccess && parsedPerPerNum != 0)
                {
                    return await _context.Offer.Take(parsedPerPerNum).ToListAsync();
                }

                var parsingMinPriceSuccess = int.TryParse(minPrice, out minPriceParsed);
                var parsingMaxPriceSuccess = int.TryParse(maxPrice, out maxPriceParsed);

                return await _context.Offer.Where(o => (offerTitle == null ? true : o.Title.Contains(offerTitle)) && ((minPrice != null && parsingMinPriceSuccess) ? minPriceParsed < o.Price : true) && ((maxPrice != null && parsingMaxPriceSuccess) ? maxPriceParsed >= o.Price : true)).ToListAsync();
            } catch (Exception e)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("/api/Offers/OwnerOffers/{id}")]
        public async Task<ActionResult<IEnumerable<Offer>>> GetOwnerOffer(int id, string offerStatus)
        {
            try
            {
                return await _context.Offer.Where(o => (offerStatus == null ? true : o.Status == offerStatus) && id == o.UserId).ToListAsync();
            }
            catch (Exception e)
            {
                return null;
            }
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
                    Status = (offer.Status == "draft" || offer.Status == "published") ? offer.Status : "draft",
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
