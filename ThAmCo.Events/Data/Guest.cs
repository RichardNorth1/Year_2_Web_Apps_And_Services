using System.ComponentModel.DataAnnotations;

namespace ThAmCo.Events.Data
{
    public class Guest
    {
        public Guest()
        {
        }

        public Guest(int guestId, string forename, string surname, int? telephone, string email, string address)
        {
            GuestId = guestId;
            Forename = forename;
            Surname = surname;
            Telephone = telephone;
            Email = email;
            Address = address;
        }

        [Key]
        public int GuestId { get; set; }
        [Required]
        public string Forename { get; set; }
        [Required]
        public string Surname { get; set; }
        public string FullName => Forename + " " + Surname;
        public int? Telephone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public List<GuestBooking> Events { get; set; }

    }
}
