namespace ThAmCo.Events.DTOs
{
    public class FoodBookingDto
    {
        public FoodBookingDto()
        {
        }

        public FoodBookingDto(FoodBookingDto fb)
        {
            FoodBookingId = fb.FoodBookingId;
            ClientReferenceId = fb.ClientReferenceId;
            NumberOfGuests = fb.NumberOfGuests;
            MenuId = fb.MenuId;
        }

        public int FoodBookingId { get; set; }

        public int ClientReferenceId { get; set; }

        public int NumberOfGuests { get; set; }

        public int MenuId { get; set; }

    }
}
