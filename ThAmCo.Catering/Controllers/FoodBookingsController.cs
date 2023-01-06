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

        #region Methods designed for adding future functionality

        /*
         * These methods are currently experimental and were designed for future improvements to the programme such 
         * as giving full food booking information to be consumed in the Events web application
         * and post and get methods utilising the client reference ID from the Events DB and have choosen to leave them in the
         * event i have time to implement them
         */



        //// GET: api/FoodBookings/FullBooking
        //[HttpGet("FullBooking")]
        //public async Task<ActionResult<IEnumerable<FullFoodBookingDto>>> GetFullBooking()
        //{
        //    var DTO = new List<FullFoodBookingDto>();
        //    var allFoodBookings = await _context.FoodBooking.ToListAsync();
        //    foreach (var foodbooking in allFoodBookings)
        //    {
        //        var Menu = await _context.Menu.FindAsync(foodbooking.MenuId);
        //        var MenuItems = await _context.MenuFoodItem.Where(m => m.MenuId == foodbooking.MenuId).ToListAsync();
        //        var FoodItems = new List<FoodItemDto>();
        //        foreach(var MenuItem in MenuItems)
        //        {
        //            var FoodItem = await _context.FoodItem.FindAsync(MenuItem.FoodItemId);
        //            FoodItems.Add(new FoodItemDto(FoodItem));
        //        }
        //        DTO.Add(new FullFoodBookingDto(foodbooking, Menu, FoodItems));
        //    }
        //    return DTO;
        //}

        //[HttpGet("FoodBookingByClientReference/{clientReference}")]
        //public async Task<ActionResult<FoodBookingDto>> GetByClientReference(int clientReference)
        //{
        //    var foodBooking = await _context.FoodBooking.Where(fb => fb.ClientReferenceId == clientReference).FirstOrDefaultAsync();


        //    if (foodBooking == null)
        //    {
        //        return NotFound();
        //    }

        //    return new FoodBookingDto(foodBooking);
        //}

        //[HttpPut("FoodBookingByClientReference/{clientReference}")]
        //public async Task<ActionResult<FoodBookingDto>> PostByClientReference(int clientReference, FoodBookingDto foodBookingDto)
        //{
        //    var foodBooking = await _context.FoodBooking.Where(fb => fb.ClientReferenceId == foodBookingDto.ClientReferenceId).FirstOrDefaultAsync();


        //    if (clientReference != foodBookingDto.ClientReferenceId)
        //    {
        //        return BadRequest();
        //    }

        //    foodBooking.MenuId = foodBookingDto.MenuId;
        //    foodBooking.NumberOfGuests = foodBookingDto.NumberOfGuests;

        //    _context.Entry(foodBooking).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!FoodBookingExists(foodBooking.FoodBookingId))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        #endregion

        #region Get Methods
        /// <summary>
        /// This method is designed to return all food bookings currently stored in the database 
        /// </summary>
        /// <returns>A collection of food bookings in a DTO format </returns>
        // GET: api/FoodBookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodBookingDto>>> GetFoodBooking()
        {
            var allFoodBookings = await _context.FoodBooking.ToListAsync();
            // Convert all food bookings to a DTO format
            var DTO = allFoodBookings.Select(fb => new FoodBookingDto(fb)).ToList();

            return DTO;
        }

        /// <summary>
        /// This method is designed to return a food booking based on the food booking id and return the food booking
        /// with the corresponding ID
        /// </summary>
        /// <param name="foodBookingId"></param>
        /// <returns> A Food booking in a DTO format</returns>
        // GET: api/FoodBookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FoodBookingDto>> GetFoodBooking(int foodBookingId)
        {
            // Find the corresponding food booking for the given ID
            var foodBooking = await _context.FoodBooking.FirstOrDefaultAsync(fb => fb.FoodBookingId == foodBookingId);

            if (foodBooking == null)
            {
                return NotFound();
            }

            return new FoodBookingDto(foodBooking);
        }

        #endregion

        #region Put Method
        /// <summary>
        /// This method is designed to take a food booking id where an update to its 
        /// values will be applied this data will be taken from the food booking DTO sent to this method  
        /// </summary>
        /// <param name="foodBookingId"></param>
        /// <param name="foodBookingDto"></param>
        /// <returns> No Content</returns>
        // PUT: api/FoodBookings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFoodBooking(int foodBookingId, FoodBookingDto foodBookingDto)
        {
            if (foodBookingId != foodBookingDto.FoodBookingId)
            {
                return BadRequest();
            }

            try
            {
                var foodBookingToUpdate = await _context.FoodBooking.FirstOrDefaultAsync(fb => fb.FoodBookingId == foodBookingId);
                if (foodBookingToUpdate == null)
                {
                    return NotFound();
                }
                // apply the updated values to the food booking
                foodBookingToUpdate.MenuId = foodBookingDto.MenuId;
                foodBookingToUpdate.NumberOfGuests = foodBookingDto.NumberOfGuests;
                foodBookingToUpdate.ClientReferenceId = foodBookingDto.ClientReferenceId;
                _context.Update(foodBookingToUpdate);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                if (!FoodBookingExists(foodBookingId))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest();
                }
            }

            return NoContent();
        }

        #endregion

        #region Post Method
        /// <summary>
        /// This method is designed to recieve a food booking Dto and transform it 
        /// into a food booking and save the newly created food booking to the database
        /// </summary>
        /// <param name="foodBookingDto"></param>
        /// <returns>The newly created food booking in a dto format</returns>
        // POST: api/FoodBookings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FoodBooking>> PostFoodBooking(FoodBookingDto foodBookingDto)
        {
            try
            {
                // create a new food Booking based off of the send DTO
                var foodBooking = new FoodBooking
                {
                    ClientReferenceId = foodBookingDto.ClientReferenceId,
                    MenuId = foodBookingDto.MenuId,
                    NumberOfGuests = foodBookingDto.NumberOfGuests

                };
                _context.FoodBooking.Add(foodBooking);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetFoodBooking", new { foodBookingId = foodBooking.FoodBookingId }, foodBooking);
            }
            catch (Exception)
            {
                if(FoodBookingExists(foodBookingDto.FoodBookingId))
                {
                    return Conflict();
                }
                else
                {
                    return BadRequest();
                }
                
                
            }        
        }

        #endregion

        #region Delete method
        /// <summary>
        /// This method is designed to take in a food booking id find the corresponding entry in the data base and remove it
        /// </summary>
        /// <param name="foodBookingId"></param>
        /// <returns> No content</returns>
        // DELETE: api/FoodBookings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFoodBooking(int foodBookingId)
        {
            var foodBooking = await _context.FoodBooking.FindAsync(foodBookingId);
            if (foodBooking == null)
            {
                return NotFound();
            }

            _context.FoodBooking.Remove(foodBooking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        #endregion

        #region Private methods
        private bool FoodBookingExists(int foodBookingId)
        {
            return _context.FoodBooking.Any(e => e.FoodBookingId == foodBookingId);
        }
        #endregion
    }
}
