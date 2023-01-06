using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ThAmCo.Events.Data;
using ThAmCo.Events.DTOs;
using ThAmCo.Venues.Data;

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
            EventName = e.EventName;
            HasFirstAider = (bool)e.HasFirstAider;
            HasRequiredStaff = (bool)e.HasRequiredStaff;
            VenueName = e.VenueName;
            Reference = e.Reference;
            EventType = e.EventType;
            EventDate = DateOnly.FromDateTime(e.EventDate).ToString();
            ClientReferenceId = e.ClientReferenceId;
            MenuForEvent = e.MenuForEvent;
            EventCost = "£" + e.EventCost;
            EventDuration = e.EventDuration;
        }

        #region EventFields
        [DisplayName("Event ID:"), Description("The Event ID For This Event")]
        public int EventId { get; set; }

        [Required, DisplayName("Event Name:"), Description("The Name Of The Event e.g The Annual Developer Conference")]
        public string EventName { get; set; }

        [DisplayName("First Aider Assigned:")]
        public bool HasFirstAider { get; set; }

        [DisplayName("Required Staff assigned:")]
        public bool HasRequiredStaff { get; set; }

        [Required, DisplayName("Event Duration in hours:")]
        public int EventDuration { get; set; }

        //[DisplayName("Event Total Cost:")]
        //public decimal EventCost { get; set; }


        [DisplayName("Event Total Cost:")]
        public string EventCost { get; set; }

        [DisplayName("Total Guests For This Booking:")]
        public int GuestCount { get; set; }

        [DisplayName("Total Staff Assigned To This Event:")]
        public int StaffCount { get; set; }

        [DisplayName("Guests Booked For This Event: ")]
        public List<GuestViewModel> GuestBookings { get; set; }

        [DisplayName("Staff Assigned To This Event: ")]
        public List<StaffViewModel> Staffing { get; set; }

        #endregion

        #region VenuesData

        [DisplayName("Venue Name: ")]
        public string VenueName { get; set; }

        [DisplayName("Booking Reference: ")]
        public string Reference { get; set; }

        [DisplayName("Event Type: ")]
        public string EventType { get; set; }

        [DisplayName("Event Date:")]
        public string EventDate { get; set; }

        #endregion

        #region CateringData

        // ID from food booking table in the catering DB
        [DisplayName("Event Food Booking Reference:")]
        public int ClientReferenceId { get; set; }

        [DisplayName("Menu For Event:")]
        public string MenuForEvent { get; set; }


        #endregion

    }
}
