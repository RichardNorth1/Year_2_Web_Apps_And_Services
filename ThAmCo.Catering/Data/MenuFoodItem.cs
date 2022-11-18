using System.ComponentModel.DataAnnotations;

namespace ThAmCo.Catering.Data
{
    public class MenuFoodItem
    {
        public MenuFoodItem()
        {
        }

        public MenuFoodItem(int menuId, int foodItemId)
        {
            MenuId = menuId;
            FoodItemId = foodItemId;
        }

        [Key]
        public int MenuId { get; set; }
        public Menu Menu { get; set; }
        [Key]
        public int FoodItemId { get; set; }
        public FoodItem Fooditem { get; set; }
    }
}
