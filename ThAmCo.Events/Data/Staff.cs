using System.ComponentModel.DataAnnotations;

namespace ThAmCo.Events.Data
{
    public class Staff
    {
        public Staff()
        {
        }

        public Staff(int staffId, string forename, 
            string surname, EmployeeType jobRole, 
            bool isFirstAider, string email, 
            string password)
        {
            StaffId = staffId;
            Forename = forename;
            Surname = surname;
            JobRole = jobRole;
            FirstAidQualified = isFirstAider;
            Email = email;
            Password = password;
        }


        [Key]
        public int StaffId { get; set; }
        [Required]
        public string Forename { get; set; }
        [Required]
        public string Surname { get; set; }
        public string FullName => Forename + " " + Surname;
        [Required]
        public bool FirstAidQualified { get;set; }
        [Required]
        public EmployeeType JobRole { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public bool IsDeleted { get; set; }
        public List<Staffing> Events { get; set; }

    }
}
