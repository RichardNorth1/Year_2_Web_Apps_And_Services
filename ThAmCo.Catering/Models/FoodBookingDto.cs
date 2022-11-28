using System.ComponentModel.DataAnnotations;
using ThAmCo.Catering.Data;

namespace ThAmCo.Catering.Models
{
    public class FoodBookingDTO
    {
        public FoodBookingDTO()
        {
        }
        public FoodBookingDTO(FoodBooking fb)
        {
            FoodBookingId = fb.FoodBookingId;
            ClientReferenceId = fb.ClientReferenceId;
            NumberOfGuests = fb.NumberOfGuests;
            MenuId = fb.MenuId;
        }
        public FoodBookingDTO(FoodBooking fb, Menu m)
        {
            FoodBookingId = fb.FoodBookingId;
            ClientReferenceId = fb.ClientReferenceId;
            NumberOfGuests = fb.NumberOfGuests;
            MenuId = fb.MenuId;
            Menu = new MenuDto(m);
        }

        public FoodBookingDTO(int foodBookingId, int clientReferenceId, int numberOfGuests, int menuId)
        {
            FoodBookingId = foodBookingId;
            ClientReferenceId = clientReferenceId;
            NumberOfGuests = numberOfGuests;
            MenuId = menuId;
        }

        public int FoodBookingId { get; set; }

        public int ClientReferenceId { get; set; }

        public int NumberOfGuests { get; set; }

        public int MenuId { get; set; }
        public MenuDto Menu { get; set; }
    }
}
