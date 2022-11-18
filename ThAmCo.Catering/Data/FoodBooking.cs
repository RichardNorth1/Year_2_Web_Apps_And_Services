using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThAmCo.Catering.Data
{
    public class FoodBooking
    {
        public FoodBooking()
        {
        }

        public FoodBooking(int foodBookingId, int clientReferenceId, int numberOfGuests, int menuId)
        {
            FoodBookingId = foodBookingId;
            ClientReferenceId = clientReferenceId;
            NumberOfGuests = numberOfGuests;
            MenuId = menuId;
        }

        [Key]
        public int FoodBookingId { get; set; }
        
        public int ClientReferenceId { get; set; }

        public int NumberOfGuests { get; set; }

        public int MenuId { get; set; }

        //note chenged from menu to Menu
        public Menu Menu { get; set; }

    }
}
