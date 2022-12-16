using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using ThAmCo.Events.Data;
using ThAmCo.Events.DTOs;

namespace ThAmCo.Events.Models
{
    public class EventViewModel
    {
        public EventViewModel()
        {
        }

        public EventViewModel(Event e)
        {
            EventId = e.EventId;
            EventDate = e.EventDate;
            EventName = e.EventName;
            VenueName = e.VenueName;
            ClientReferenceId = e.ClientReferenceId;
            Reference = e.Reference;
            HasRequiredStaff = (bool)e.HasRequiredStaff;
            HasFirstAider = (bool)e.HasFirstAider;
        }

        //public int EventId { get; set; }
        //public int MenuId { get; set; }
        //public string VenueName { get; set; }
        //public int GuestCount { get; set; }
        //public int StaffCount { get; set; }
        //public DateOnly EventDateStart { get; set; }
        //public DateOnly EventDateEnd { get; set; }
        //public DateTime EventDate { get; set; }
        //public string EventName { get; set; }
        //public int ClientReferenceId { get; set; }
        //public string Reference { get; set; }
        //public string EventTypeId { get; set; }

        //public bool? HasFirstAider { get; set; }
        //public bool? HasRequiredStaff { get; set; }
        //public List<GuestBookingViewModel> GuestBookings { get; set; }
        //public List<StaffingViewModel> Staffs { get; set; }

        public int MenuId { get; set; }
        public DateOnly EventDateStart { get; set; }
        public DateOnly EventDateEnd { get; set; }
        public string SelectedEventDate { get; set; }
        public string SelectedEventTypeId { get; set; }
        public string SelectedMenuId { get; set; }

        #region EventFields

        public int EventId { get; set; }


        public string EventName { get; set; }

        public bool HasFirstAider { get; set; }

        public bool HasRequiredStaff { get; set; }

        public bool IsDeleted { get; set; }
        public int GuestCount { get; set; }
        public int StaffCount { get; set; }

        public List<GuestBooking> GuestBookings { get; set; }

        public List<Staffing> Staffs { get; set; }

        #endregion

        #region VenuesData

        public string VenueName { get; set; }

        public string Reference { get; set; }


        public string EventTypeId { get; set; } = string.Empty;

        public DateTime EventDate { get; set; }

        #endregion

        #region CateringData

        // ID from food booking table in the catering DB
        public int ClientReferenceId { get; set; }

        public string MenuForEvent { get; set; }

        #endregion

    }
}
