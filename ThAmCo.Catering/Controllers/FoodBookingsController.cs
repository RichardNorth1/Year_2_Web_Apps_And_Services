using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Catering.Data;
using ThAmCo.Catering.Models;

namespace ThAmCo.Catering.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodBookingsController : ControllerBase
    {
        private readonly CateringDBContext _context;

        public FoodBookingsController(CateringDBContext context)
        {
            _context = context;
        }

        // GET: api/FoodBookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodBookingDTO>>> GetFoodBooking()
        {
            var allFoodBookings = await _context.FoodBooking.ToListAsync();
            var DTO = allFoodBookings.Select(fb => new FoodBookingDTO
            {
                FoodBookingId = fb.FoodBookingId,
                ClientReferenceId = fb.ClientReferenceId,
                NumberOfGuests = fb.NumberOfGuests,
                MenuId = fb.MenuId
            }).ToList();

            return DTO;
        }

        // GET: api/FoodBookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FoodBookingDTO>> GetFoodBooking(int id)
        {
            var foodBooking = await _context.FoodBooking.FindAsync(id);

            if (foodBooking == null)
            {
                return NotFound();
            }

            var foodBookingDto = new FoodBookingDTO
            {
                FoodBookingId = foodBooking.FoodBookingId,
                ClientReferenceId = foodBooking.ClientReferenceId,
                NumberOfGuests = foodBooking.NumberOfGuests,
                MenuId = foodBooking.MenuId

            };

            return foodBookingDto;
        }

        // PUT: api/FoodBookings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFoodBooking(int id, FoodBooking foodBooking)
        {
            if (id != foodBooking.FoodBookingId)
            {
                return BadRequest();
            }

            _context.Entry(foodBooking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FoodBookingExists(id))
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

        // POST: api/FoodBookings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FoodBooking>> PostFoodBooking(FoodBooking foodBooking)
        {
            _context.FoodBooking.Add(foodBooking);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFoodBooking", new { id = foodBooking.FoodBookingId }, foodBooking);
        }

        // DELETE: api/FoodBookings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFoodBooking(int id)
        {
            var foodBooking = await _context.FoodBooking.FindAsync(id);
            if (foodBooking == null)
            {
                return NotFound();
            }

            _context.FoodBooking.Remove(foodBooking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FoodBookingExists(int id)
        {
            return _context.FoodBooking.Any(e => e.FoodBookingId == id);
        }
    }
}
