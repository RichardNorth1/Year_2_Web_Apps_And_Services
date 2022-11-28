using System.ComponentModel.DataAnnotations;
using ThAmCo.Venues.Data;

namespace ThAmCo.Events.DTOs
{
    public class EventTypeDto
    {
        public EventTypeDto()
        {
        }

        public EventTypeDto(string id, string title)
        {
            Id = id;
            Title = title;
        }

        public string Id { get; set; }

        public string Title { get; set; }
    }
}
