using Microsoft.Extensions.Logging;
using System.Security.Cryptography.X509Certificates;
using ThAmCo.Events.Data;

namespace ThAmCo.Events.DTOs
{
    public class GuestDto
    {
        public GuestDto()
        {
        }


        public GuestDto(Guest g)
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

    }
}
