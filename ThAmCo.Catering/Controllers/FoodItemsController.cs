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
    public class FoodItemsController : ControllerBase
    {
        private readonly CateringDBContext _context;

        public FoodItemsController(CateringDBContext context)
        {
            _context = context;
        }
        #region Get Methods
        /// <summary>
        /// This method is designed to return a list of all Food items in the database in a DTO format
        /// </summary>
        /// <returns> a collection of food item DTOs</returns>
        // GET: api/FoodItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodItemDto>>> GetFoodItem()
        {
            
            var allFoodItems = await _context.FoodItem.ToListAsync();
            // create a list of food item dtos
            var DTOs = allFoodItems.Select(fb => new FoodItemDto(fb)).ToList();

            return DTOs;
        }

        /// <summary>
        /// This method is used to return a Food item Dto by specifying the ID of the food item
        /// </summary>
        /// <param name="foodItemId"></param>
        /// <returns></returns>
        // GET: api/FoodItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FoodItemDto>> GetFoodItem(int foodItemId)
        {
            // find the food item
            var foodItem = await _context.FoodItem.FirstOrDefaultAsync( fi => fi.FoodItemId == foodItemId);
            // if it doesnt exist return not found
            if (foodItem == null)
            {
                return NotFound();
            }
            // change the food item to a DTO
            var DTO = new FoodItemDto(foodItem);

            return DTO;
        }

        #endregion

        #region Put Method
        /// <summary>
        /// This method is designed to take a food item id that is to be updated and the new values for the food item
        /// </summary>
        /// <param name="foodItemId"></param>
        /// <param name="foodItemDto"></param>
        /// <returns> No content</returns>
        // PUT: api/FoodItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFoodItem(int foodItemId, FoodItemDto foodItemDto)
        {
            if (foodItemId != foodItemDto.FoodItemId)
            {
                return BadRequest();
            }



            try
            {
                var foodItem = await _context.FoodItem
                    .Where(fi => fi.FoodItemId == foodItemId)
                    .FirstOrDefaultAsync();
                if (foodItem == null)
                {
                    return NotFound();
                }
                foodItem.Description = foodItemDto.Description;
                foodItem.UnitPrice = foodItemDto.UnitPrice;
                _context.Update(foodItem);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                if (!FoodItemExists(foodItemId))
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
        /// This Method is designed to take in a Food item Dto and create a new food item and save it to the database
        /// </summary>
        /// <param name="foodItemDto"></param>
        /// <returns></returns>
        // POST: api/FoodItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FoodItem>> PostFoodItem(FoodItemDto foodItemDto)
        {
            try
            {
                var newFoodItem = new FoodItem { Description = foodItemDto.Description, UnitPrice = foodItemDto.UnitPrice };
                _context.FoodItem.Add(newFoodItem);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetFoodItem", new { foodItemId = newFoodItem.FoodItemId }, newFoodItem);
            }
            catch (Exception)
            {
                if (FoodItemExists(foodItemDto.FoodItemId)) 
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

        #region Delete Method

        /// <summary>
        /// This Method is designed to take the ID of the food item that is to be deleted and remove the entry from the database
        /// </summary>
        /// <param name="foodItemId"></param>
        /// <returns>No content</returns>
        // DELETE: api/FoodItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFoodItem(int foodItemId)
        {
            var foodItem = await _context.FoodItem.FirstOrDefaultAsync(fi => fi.FoodItemId == foodItemId);
            if (foodItem == null)
            {
                return NotFound();
            }

            _context.FoodItem.Remove(foodItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// This method is designed to determine wether a food item exists in the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true if the food item exists otherwise returns false</returns>
        private bool FoodItemExists(int id)
        {
            return _context.FoodItem.Any(e => e.FoodItemId == id);
        }

        #endregion
    }
}
