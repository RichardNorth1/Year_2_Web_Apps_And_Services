using ThAmCo.Events.Data;

namespace ThAmCo.Events.Models
{
    public class GuestBookingViewModel
    {
        public GuestBookingViewModel()
        {
        }

        public GuestBookingViewModel(GuestBooking gb)
        {
            GuestId = gb.GuestId;
            EventId = gb.EventId;
            Guest = new GuestViewModel(gb.Guest);
            Event = new EventViewModel(gb.Event);
        }


        public int GuestId { get; set; }
        public GuestViewModel Guest { get; set; }
        public int EventId { get; set; }
        public EventViewModel Event { get; set; }
    }
}
