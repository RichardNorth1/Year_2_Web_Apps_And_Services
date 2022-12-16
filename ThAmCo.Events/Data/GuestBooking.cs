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

        public bool Attended { get; set; }
        public int GuestId { get; set; }
        public Guest Guest { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
        public bool IsDeleted { get; set; }
    }
}
