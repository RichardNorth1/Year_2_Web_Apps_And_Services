using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using ThAmCo.Events.Data;
using ThAmCo.Events.DTOs;

namespace ThAmCo.Events.Models.EventViewModels
{
    public class CreateEventViewModel : EventViewModel
    {
        public CreateEventViewModel()
        {
        }


        public int EventId { get; set; }
        public string EventName { get; set; }
        public int MenuId { get; set; }
        public int ClientReferenceId { get; set; }
        public string Reference { get; set; }
        public string VenueName { get; set; }
        public string EventTypeId { get; set; }
        public DateOnly EventDateStart { get; set; }
        public DateOnly EventDateEnd { get; set; }
        public DateTime EventDate { get; set; }

    }
}
