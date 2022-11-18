using System.ComponentModel.DataAnnotations;

namespace ThAmCo.Catering.Data
{
    public class FoodItem
    {
        public FoodItem()
        {
        }

        public FoodItem(int foodItemId, string description, double unitPrice)
        {
            FoodItemId = foodItemId;
            Description = description;
            UnitPrice = unitPrice;
        }

        [Key]
        public int FoodItemId { get; set; }
        [Required,MaxLength(50)]
        public string Description { get; set; }

        public double UnitPrice { get; set; }
        public List<MenuFoodItem> Menus { get; set; }
    }
}
