using Microsoft.AspNetCore.Mvc.Rendering;
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



        public int MenuId { get; set; }
        public DateOnly EventDateStart { get; set; }
        public DateOnly EventDateEnd { get; set; }
        public string SelectedEventDate { get; set; }
        public string SelectedEventType { get; set; }
        public string SelectedMenuId { get; set; }
    }
}
