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
        // GET: api/FoodBookings/FullBooking
        [HttpGet("FullBooking")]
        public async Task<ActionResult<IEnumerable<FullFoodBookingDto>>> GetFullBooking()
        {
            var DTO = new List<FullFoodBookingDto>();
            var allFoodBookings = await _context.FoodBooking.ToListAsync();
            foreach (var foodbooking in allFoodBookings)
            {
                var Menu = await _context.Menu.FindAsync(foodbooking.MenuId);
                var MenuItems = await _context.MenuFoodItem.Where(m => m.MenuId == foodbooking.MenuId).ToListAsync();
                var FoodItems = new List<FoodItemDto>();
                foreach(var MenuItem in MenuItems)
                {
                    var FoodItem = await _context.FoodItem.FindAsync(MenuItem.FoodItemId);
                    FoodItems.Add(new FoodItemDto(FoodItem));
                }
                DTO.Add(new FullFoodBookingDto(foodbooking, Menu, FoodItems));
            }
            return DTO;
        }

        // GET: api/FoodBookings/FullBooking/5
        [HttpGet("FullBooking/{foodBookingId}")]
        public async Task<ActionResult<FullFoodBookingDto>> GetFullBooking(int foodBookingId)
        {
            var FoodBooking = await _context.FoodBooking.FindAsync(foodBookingId);

            var Menu = await _context.Menu.FindAsync(FoodBooking.MenuId);
            var MenuItems = await _context.MenuFoodItem.Where(m => m.MenuId == Menu.MenuId).ToListAsync();
            var FoodItems = new List<FoodItemDto>();
            foreach (var MenuItem in MenuItems)
            {
                var FoodItem = await _context.FoodItem.FindAsync(MenuItem.FoodItemId);
                FoodItems.Add(new FoodItemDto(FoodItem));
            }
            var DTO = new FullFoodBookingDto(FoodBooking, Menu, FoodItems);
            
            return DTO;
        }

        // GET: api/FoodBookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodBookingDTO>>> GetFoodBooking()
        {
            var allFoodBookings = await _context.FoodBooking.ToListAsync();
            var DTO = allFoodBookings.Select(fb => new FoodBookingDTO(fb)).ToList();

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

            return new FoodBookingDTO(foodBooking);
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
