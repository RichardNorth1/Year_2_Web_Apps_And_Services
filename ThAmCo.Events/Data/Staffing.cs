namespace ThAmCo.Events.Data
{
    public class Staffing
    {
        public Staffing()
        {
        }

        public Staffing(int staffId, int eventId)
        {
            StaffId = staffId;
            EventId = eventId;
        }

        public int StaffId { get; set; }
        public Staff Staff { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
    }
}
