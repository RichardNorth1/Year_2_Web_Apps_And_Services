using ThAmCo.Events.DTOs;

namespace ThAmCo.Events.Models.DTOs
{
    public class FoodBookingViewModel
    {
        public FoodBookingViewModel()
        {
        }

        public FoodBookingViewModel(FullFoodBookingDto fb)
        {

            ClientReferenceId = fb.ClientReferenceId;
            NumberOfGuests = fb.NumberOfGuests;
            MenuId = fb.MenuId;
            MenuName = fb.MenuName;
            MenuItems = fb.MenuItems;
         }

        public int FoodBookingId { get; set; }
        public int ClientReferenceId { get; set; }

        public int NumberOfGuests { get; set; }

        public int MenuId { get; set; }

        
        public string MenuName { get; set; }
        public List<FoodItemDto> MenuItems { get; set; }
    }
}
