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
    public class MenuFoodItemsController : ControllerBase
    {
        private readonly CateringDBContext _context;

        public MenuFoodItemsController(CateringDBContext context)
        {
            _context = context;
        }

        #region Get Methods

        /// <summary>
        /// This Method is designed to return all Menu food items currently stored in the database as a menu food item dto
        /// </summary>
        /// <returns>menu food item DTO</returns>
        // GET: api/MenuFoodItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuFoodItemDto>>> GetMenuFoodItem()
        {
            var allMenuFoodItems = await _context.MenuFoodItem.ToListAsync();
            var DTO = allMenuFoodItems.Select(amfi => new MenuFoodItemDto(amfi)).ToList();

            return DTO;
        }

        /// <summary>
        /// This Method is designed to return a specific menu food item in the format of a menu food item DTO
        /// </summary>
        /// <param name="id1"></param>
        /// <param name="id2"></param>
        /// <returns></returns>
        // GET: api/MenuFoodItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MenuFoodItemDto>> GetMenuFoodItem(int menuId, int foodItemId)
        {
            var MenuFoodItem = await _context.MenuFoodItem
                .Where(mfi => mfi.MenuId == menuId && mfi.FoodItemId == foodItemId)
                .FirstOrDefaultAsync();

            if (MenuFoodItem == null)
            {
                return NotFound();
            }

            return new MenuFoodItemDto(MenuFoodItem);

        }
        #endregion

        #region Post Method

        /// <summary>
        /// This method is designed to take in a menu food item DTO  and create a new menu food item and save it to the database
        /// </summary>
        /// <param name="menuFoodItemDto"></param>
        /// <returns></returns>
        // POST: api/MenuFoodItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MenuFoodItem>> PostMenuFoodItem(MenuFoodItemDto menuFoodItemDto)
        {

            try
            {
                var menuFoodItem = new MenuFoodItem { FoodItemId = menuFoodItemDto.FoodItemId, MenuId = menuFoodItemDto.MenuId };
                _context.MenuFoodItem.Add(menuFoodItem);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetMenuFoodItem", new { menuId = menuFoodItem.MenuId, foodItemId = menuFoodItem.FoodItemId }, menuFoodItem);
            }
            catch (Exception)
            {
                if (MenuFoodItemExists(menuFoodItemDto.MenuId , menuFoodItemDto.FoodItemId))
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
        /// This method is designed to take a menuId and foodItemId for the menu food item to be deleted and remove it from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/MenuFoodItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenuFoodItem(int menuId, int foodItemId)
        {
            var menuFoodItem = await _context.MenuFoodItem.Where(mfi => mfi.MenuId == menuId && mfi.FoodItemId == foodItemId).FirstOrDefaultAsync();
            if (menuFoodItem == null)
            {
                return NotFound();
            }

            _context.MenuFoodItem.Remove(menuFoodItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        #endregion

        #region Private Methods
        private bool MenuFoodItemExists(int menuId, int foodItemId)
        {
            return _context.MenuFoodItem.Any(e => e.MenuId == menuId && e.FoodItemId == foodItemId);
        }
        #endregion
    }
}
