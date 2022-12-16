using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ThAmCo.Venues.Data;


namespace ThAmCo.Events.Data
{
    public class Event
    {
        public Event()
        {
        }

        public Event(int eventId, string eventName,
            int clientReferenceId, string reference,
            string eventTypeId)
        {
            EventId = eventId;
            EventName = eventName;
            ClientReferenceId = clientReferenceId;
            Reference = reference;
            EventTypeId = eventTypeId;
        }



        //#region EventFields

        //[Key]
        //public int EventId { get; set; }
        //public DateTime EventDate { get; set; }

        //[Required]
        //public string EventName { get; set; }

        //public bool? HasFirstAider { get; set; }

        //public bool? HasRequiredStaff { get; set; }

        //public bool IsDeleted { get; set; }

        //public List<GuestBooking> GuestBookings { get; set; }

        //public List<Staffing> Staffs { get; set; }

        //#endregion

        //#region VenuesData

        //[Required]
        //public string VenueName { get; set; }

        //[MinLength(13), MaxLength(13)]
        //public string Reference { get; set; }
        //[Required]
        //public string EventType { get; set; }



        //#endregion

        //#region CateringData

        //// ID from food booking table in the catering DB
        //public int ClientReferenceId { get; set; }

        //#endregion

        [Key]
        public int EventId { get; set; }

        [Required]
        public string EventName { get; set; }

        public bool? HasFirstAider { get; set; }

        public bool? HasRequiredStaff { get; set; }

        public bool IsDeleted { get; set; }

        public List<GuestBooking> GuestBookings { get; set; }

        public List<Staffing> Staffs { get; set; }


        public string VenueName { get; set; }

        [MinLength(13), MaxLength(13)]
        public string Reference { get; set; }

        [MinLength(3), MaxLength(3)]
        public string EventTypeId { get; set; } = string.Empty;
        [Required]
        public DateTime EventDate { get; set; }

        // ID from food booking table in the catering DB
        public int ClientReferenceId { get; set; }

        public string MenuForEvent { get; set; }


    }
}
