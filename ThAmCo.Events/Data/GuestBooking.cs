namespace ThAmCo.Events.Data
{
    public class GuestBooking
    {
        public GuestBooking()
        {
        }

        public GuestBooking(int guestId, int eventId)
        {
            GuestId = guestId;
            EventId = eventId;
        }

        public int GuestId { get; set; }
        public Guest Guest { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
    }
}
