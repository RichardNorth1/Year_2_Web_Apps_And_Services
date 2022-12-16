using Microsoft.AspNetCore.Mvc.Rendering;
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
            Event = new EventViewModel(s.Event);
            Staff = new StaffViewModel(s.Staff);
        }

        public StaffingViewModel(int staffId, int eventId)
        {
            StaffId = staffId;
            EventId = eventId;
        }

        public int StaffId { get; set; }
        public StaffViewModel Staff { get; set; }
        public int EventId { get; set; }
        public EventViewModel Event { get; set; }
        public string SelectedStaff { get; set; }
        public List<SelectListItem> StaffSelectList { get; set; }
    }
}
