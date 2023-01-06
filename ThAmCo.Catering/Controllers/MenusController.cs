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
    public class MenusController : ControllerBase
    {
        private readonly CateringDBContext _context;

        public MenusController(CateringDBContext context)
        {
            _context = context;
        }
        #region Get Methods
        /// <summary>
        /// This method is designed to return all Menus from the database in th form of a dTO
        /// </summary>
        /// <returns>a collection of Menu DTOs</returns>
        // GET: api/Menus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuDto>>> GetMenu()
        {
            var menus = await _context.Menu.ToListAsync();
            var DTO = menus.Select(m => new MenuDto(m)).ToList();

            return DTO;
        }

        /// <summary>
        /// This method is designed to return a Dto based off of the Menu ID supplied to the method
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A menu DTo</returns>
        // GET: api/Menus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MenuDto>> GetMenu(int menuId)
        {
            // find the Menu that is being searched for
            var menu = await _context.Menu.FirstOrDefaultAsync(m => m.MenuId == menuId);

            if (menu == null)
            {
                return NotFound();
            }

            return new MenuDto(menu);

        }
        #endregion

        #region Put Methods

        /// <summary>
        /// This method is designed to take in a menu Id that the user wishes to edit and the details the wish to update in the Menu
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="menu"></param>
        /// <returns></returns>
        // PUT: api/Menus/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMenu(int menuId, MenuDto menuDto)
        {
            if (menuId != menuDto.MenuId)
            {
                return BadRequest();
            }

            try
            {
                var menu =await _context.Menu
                    .Where(m => m.MenuId == menuId)
                    .FirstOrDefaultAsync();
                if (menu == null)
                {
                    return NotFound();
                }
                menu.MenuName = menuDto.MenuName;
                
                _context.Update(menu);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                if (!MenuExists(menuId))
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
        /// This method is designed to take a menu dto and the create a new menu based from this information and save the new entry to the database
        /// </summary>
        /// <param name="menuDto"></param>
        /// <returns></returns>
        // POST: api/Menus
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Menu>> PostMenu(MenuDto menuDto)
        {
            try
            {
                var newMenu = new Menu { MenuName = menuDto.MenuName };
                _context.Menu.Add(newMenu);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetMenu", new { menuId = newMenu.MenuId }, menuDto);
            }
            catch (Exception)
            {
                if (MenuExists(menuDto.MenuId))
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
        /// This Method is design to take a menu ID that the user wishes to delete and removes it from the database
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        // DELETE: api/Menus/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenu(int menuId)
        {
            var menu = await _context.Menu.Where(m => m.MenuId == menuId).FirstOrDefaultAsync();
            if (menu == null)
            {
                return NotFound();
            }

            _context.Menu.Remove(menu);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        #endregion

        #region Private Methods

        private bool MenuExists(int id)
        {
            return _context.Menu.Any(e => e.MenuId == id);
        }
        #endregion
    }
}
