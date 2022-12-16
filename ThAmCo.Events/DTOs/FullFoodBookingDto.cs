namespace ThAmCo.Events.DTOs
{
    public class FullFoodBookingDto
    {
        public FullFoodBookingDto()
        {
        }

        public FullFoodBookingDto(FullFoodBookingDto fb)
        {
            FoodBookingId = fb.FoodBookingId;
            ClientReferenceId = fb.ClientReferenceId;
            NumberOfGuests = fb.NumberOfGuests;
            MenuId = fb.MenuId;
            MenuName = fb.MenuName;
            MenuItems = fb.MenuItems;
        }
        public int FoodBookingId { get; internal set; }
        public int ClientReferenceId { get; set; }

        public int NumberOfGuests { get; set; }

        public int MenuId { get; set; }

        //menu

        public string MenuName { get; set; }
        public List<FoodItemDto> MenuItems { get; set; }

    }
}
