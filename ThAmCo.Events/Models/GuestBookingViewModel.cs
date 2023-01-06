using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
            Attended = gb.Attended ? "Attended" : "Did Not Attend";
            AttendenceList = new List<SelectListItem> { 
                new SelectListItem { Text = "Yes", Value = "yes" },
                new SelectListItem { Text = "No", Value = "no" } };
        }

        [DisplayName("Guest Attendence")]
        public string Attended { get; set; }
        public int GuestId { get; set; }
        public GuestViewModel Guest { get; set; }
        public int EventId { get; set; }
        [Required,DisplayName("Guest Selection")]
        public string SelectedGuestId { get; set; }
        public EventViewModel Event { get; set; }
        public List<SelectListItem> AttendenceList { get; set; }
        public List<SelectListItem> GuestSelectList { get; set; }
    }
}
