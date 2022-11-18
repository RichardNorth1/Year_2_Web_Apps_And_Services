using ThAmCo.Events.Data;

namespace ThAmCo.Events.DTOs
{
    public class StaffDTO
    {




        public int StaffId { get; set; }


        public string FullName { get; set; }

        public EmployeeType JobRole { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public List<Staffing> Events { get; set; }

    }
}
