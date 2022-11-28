using System.ComponentModel.DataAnnotations;
using ThAmCo.Catering.Data;

namespace ThAmCo.Catering.Models
{
    public class MenuFoodItemDto
    {
        public MenuFoodItemDto()
        {
        }

        public MenuFoodItemDto(MenuFoodItem mfi)
        {
            MenuId = mfi.MenuId;
            Menu = mfi.Menu;
            FoodItemId = mfi.FoodItemId;
            FoodItem = mfi.Fooditem;
        }

        public int MenuId { get; set; }
        public Menu Menu { get; set; }
        public int FoodItemId { get; set; }
        public FoodItem FoodItem { get; set; }

    }
}
