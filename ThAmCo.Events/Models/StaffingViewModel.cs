using ThAmCo.Events.Data;

namespace ThAmCo.Events.Models
{
    public class StaffingViewModel
    {
        public StaffingViewModel()
        {
        }

        public StaffingViewModel(Staffing s)
        {
            StaffId = s.StaffId;
            EventId = s.EventId;
        }

        public StaffingViewModel(int staffId, int eventId)
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
