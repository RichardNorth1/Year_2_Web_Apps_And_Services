using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using ThAmCo.Venues.Data;


namespace ThAmCo.Events.Data
{
    public class Event
    {
        public Event()
        {
        }

        public Event(int eventId, int clientReferenceId)
        {
            EventId = eventId;
            ClientReferenceId = clientReferenceId;
        }

        [Key]
        public int EventId { get; set; }
        // ID from food booking table in the catering DB
        public int ClientReferenceId { get; set; }
        // Id from the event type table in venues DB
        [MinLength(13), MaxLength(13)]
        public string Reference { get; set; }

        [MinLength(3), MaxLength(3)]
        public string EventTypeId { get; set; } = string.Empty;


        public List<GuestBooking> GuestBookings{ get; set; }
        public List<Staffing> Staffs { get; set; }
    }
}
