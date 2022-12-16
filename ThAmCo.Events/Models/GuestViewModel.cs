using ThAmCo.Events.Data;

namespace ThAmCo.Events.Models
{
    public class GuestViewModel
    {
        public GuestViewModel()
        {
        }
        public GuestViewModel(Guest g)
        {
            GuestId = g.GuestId;
            Forename = g.Forename;
            Surname = g.Surname;
            Telephone = g.Telephone;
            Email = g.Email;
            Address = g.Address;

        }

        public int GuestId { get; set; }

        public string Forename { get; set; }

        public string Surname { get; set; }
        public string FullName => Forename + " " + Surname;
        public int? Telephone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public List<GuestBookingViewModel> Bookings { get; set; }
    }
}
