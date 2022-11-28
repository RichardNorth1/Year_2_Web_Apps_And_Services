namespace ThAmCo.Events.DTOs
{
    public class MenuFoodItemDto
    {
        public MenuFoodItemDto()
        {
        }

        public MenuFoodItemDto(MenuFoodItemDto mfi)
        {
            MenuId = mfi.MenuId;
            FoodItemId = mfi.FoodItemId;
        }

        public int MenuId { get; set; }
        public int FoodItemId { get; set; }

    }
}
