using System.ComponentModel.DataAnnotations;
using ThAmCo.Catering.Data;

namespace ThAmCo.Catering.Models
{
    public class FoodItemDto
    {

        [Key]
        public int FoodItemId { get; set; }
        [Required, MaxLength(50)]
        public string Description { get; set; }

        public double UnitPrice { get; set; }
        public List<MenuFoodItem> Menus { get; set; }

    }
}
