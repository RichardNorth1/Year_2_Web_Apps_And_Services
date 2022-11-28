using ThAmCo.Events.Data;

namespace ThAmCo.Events.Models
{
    public class StaffViewModel
    {
        public StaffViewModel()
        {
        }

        public StaffViewModel(Staff s)
        {
            StaffId = s.StaffId;
            FullName = s.FullName;
            JobRole = s.JobRole;
            Email = s.Email;
            Password = s.Password;
        }

        public StaffViewModel(int staffId, string fullName, 
            EmployeeType jobRole, string email, 
            string password, List<Staffing> events)
        {
            StaffId = staffId;
            FullName = fullName;
            JobRole = jobRole;
            Email = email;
            Password = password;
            Events = events;
        }

        public int StaffId { get; set; }
        public string FullName { get; set; }
        public EmployeeType JobRole { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Staffing> Events { get; set; }

    }
}
