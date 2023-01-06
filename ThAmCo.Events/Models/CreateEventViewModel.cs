using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ThAmCo.Events.Data;
using ThAmCo.Events.DTOs;

namespace ThAmCo.Events.Models
{
    public class CreateEventViewModel : EventViewModel
    {
        public CreateEventViewModel()
        {
        }

        public CreateEventViewModel(Event eventData) : base(eventData)
        {
        }


        [Required,DisplayName("Menu")]
        public int MenuId { get; set; }

        [Required, DisplayName("Event Date Start")]
        public DateOnly EventDateStart { get; set; }

        [Required, DisplayName("Event Date End")]
        public DateOnly EventDateEnd { get; set; }

        [Required, DisplayName("Number Of Guests For Food Booking")]
        public int NumberOfGuestsForFoodBooking { get; set; }

        [Required, DisplayName("Event Date")]
        public string SelectedEventDate { get; set; }
        [Required, DisplayName("Event Type")]
        public string SelectedEventType { get; set; }
        [Required, DisplayName("Menu Name")]
        public string SelectedMenuIdAndName { get; set; }

    }
}
