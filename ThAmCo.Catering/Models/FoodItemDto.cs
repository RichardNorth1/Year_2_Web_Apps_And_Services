using System.ComponentModel.DataAnnotations;
using ThAmCo.Catering.Data;

namespace ThAmCo.Catering.Models
{
    public class FoodItemDto
    {
        public FoodItemDto()
        {
        }

        //public FoodItemDto(int foodItemId, string description, double unitPrice)
        //{
        //    FoodItemId = foodItemId;
        //    Description = description;
        //    UnitPrice = unitPrice;
        //}

        public FoodItemDto(FoodItem fi)
        {
            FoodItemId = fi.FoodItemId;
            Description = fi.Description;
            UnitPrice = fi.UnitPrice;

        }
        public int FoodItemId { get; set; }

        public string Description { get; set; }

        public double UnitPrice { get; set; }
        //public List<MenuFoodItemDto> MenuDtos { get; set; }

    }
}
