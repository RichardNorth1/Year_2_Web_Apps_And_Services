namespace ThAmCo.Events.DTOs
{
    public class FoodItemDto
    {
        public FoodItemDto()
        {
        }

        public FoodItemDto(FoodItemDto fi)
        {
            FoodItemId = fi.FoodItemId;
            Description = fi.Description;
            UnitPrice = fi.UnitPrice;
        }

        public int FoodItemId { get; set; }

        public string Description { get; set; }

        public double UnitPrice { get; set; }

    }
}
