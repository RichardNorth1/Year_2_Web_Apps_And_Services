using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ThAmCo.Venues.Data;


namespace ThAmCo.Events.Data
{
    public class Event
    {
        /// <summary>
        /// 
        /// </summary>
        public Event()
        {
        }

        public Event(int eventId, string eventName,
            int clientReferenceId, string reference, 
            decimal price, int duration)
        {
            EventId = eventId;
            EventName = eventName;
            ClientReferenceId = clientReferenceId;
            Reference = reference;
            EventCost = price;
            EventDuration = duration;
        }

        [Key]
        public int EventId { get; set; }

        [Required]
        public string EventName { get; set; }

        public int EventDuration { get; set; }
        public decimal EventCost { get; set; }

        public bool? HasFirstAider { get; set; }

        public bool? HasRequiredStaff { get; set; }

        public bool IsDeleted { get; set; }

        public List<GuestBooking> GuestBookings { get; set; }

        public List<Staffing> Staffs { get; set; }


        public string VenueName { get; set; }


        public string Reference { get; set; }


        public string EventType { get; set; }


        public DateTime EventDate { get; set; }

        // ID from food booking table in the catering DB
        public int ClientReferenceId { get; set; }

        public string MenuForEvent { get; set; }


    }
}
