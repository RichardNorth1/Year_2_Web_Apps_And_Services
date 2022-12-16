using System.ComponentModel.DataAnnotations;
using ThAmCo.Catering.Data;

namespace ThAmCo.Catering.Models
{
    public class MenuDto
    {
        public MenuDto()
        {
        }

        public MenuDto(Menu m)
        {
            MenuId = m.MenuId;
            MenuName = m.MenuName;
        }

        //public MenuDto(int menuId, string menuName, List<MenuFoodItemDto> menuItemsDto)
        //{
        //    MenuId = menuId;
        //    MenuName = menuName;
        //    MenuItemsDto = menuItemsDto;
        //}

        public int MenuId { get; set; }

        public string MenuName { get; set; }
        //public List<MenuFoodItemDto> MenuItemsDto { get; set; }
    }
}
