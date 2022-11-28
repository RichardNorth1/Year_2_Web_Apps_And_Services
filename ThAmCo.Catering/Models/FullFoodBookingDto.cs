using ThAmCo.Catering.Data;

namespace ThAmCo.Catering.Models
{
    public class FullFoodBookingDto
    {
        public FullFoodBookingDto(FoodBooking fb, Menu m, List<FoodItemDto> menuItems)
        {
            FoodBookingId = fb.FoodBookingId;
            ClientReferenceId = fb.ClientReferenceId;
            NumberOfGuests = fb.NumberOfGuests;
            MenuId = fb.MenuId;
            MenuName = m.MenuName;
            MenuItems = menuItems;
        }
        public int FoodBookingId { get; set; }
        public int ClientReferenceId { get; set; }

        public int NumberOfGuests { get; set; }

        public int MenuId { get; set; }

        //menu

        public string MenuName { get; set; }
        public List<FoodItemDto> MenuItems { get; set; }
    }
}
