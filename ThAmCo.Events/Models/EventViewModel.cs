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
        public EventViewModel(IEnumerable<EventTypeDto> et)
        {
            EventTypeList = createSelectList(et);
        }
        public EventViewModel(Event e, IEnumerable<EventTypeDto> et)
        {
            EventId = e.EventId;
            EventName = e.EventName;
            ClientReferenceId = e.ClientReferenceId;
            Reference = e.Reference;
            EventTypeId = e.EventTypeId;
            EventTypeList = createSelectList(et);
        }

        public EventViewModel(Event e)
        {
            EventId = e.EventId;
            EventName = e.EventName;
            ClientReferenceId = e.ClientReferenceId;
            Reference = e.Reference;
            EventTypeId = e.EventTypeId;
        }

        public int EventId { get; set; }
        public DateOnly EventDateStart { get; set; }
        public DateOnly EventDateEnd { get; set; }
        public DateOnly EventDate { get; set; }
        public string EventName { get; set; }
        public int ClientReferenceId { get; set; }
        public string Reference { get; set; }
        public string EventTypeId { get; set; }
        public List<SelectListItem> EventTypeList { get; set; }
        public List<GuestBookingViewModel> GuestBookings { get; set; }
        public List<StaffingViewModel> Staffs { get; set; }

        private List<SelectListItem> createSelectList(IEnumerable<EventTypeDto> et)
        {
            var list = new List<SelectListItem>();
            var eventSelectList = new List<SelectListItem>(et.Select(e => new SelectListItem
            {
                Value = e.Id,
                Text = e.Title
            }));
            return eventSelectList;
        }
    }
}
