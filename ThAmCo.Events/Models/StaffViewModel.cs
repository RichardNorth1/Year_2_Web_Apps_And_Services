using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ThAmCo.Events.Data;

namespace ThAmCo.Events.Models
{
    public class StaffViewModel
    {
        public StaffViewModel()
        {
            QualifiedSelectList = new List<SelectListItem> {
                new SelectListItem { Text = "Yes", Value = "yes" },
                new SelectListItem { Text = "No", Value = "no" } };

            ListOfJobRoles = new List<SelectListItem> {
                new SelectListItem { Text = "Manager", Value = "Manager" },
                new SelectListItem { Text = "Team Leader", Value = "Team Leader" },
                new SelectListItem { Text = "Team Member", Value = "Team Member" } };

        }

        public StaffViewModel(Staff s)
        {
            StaffId = s.StaffId;
            Forename = s.Forename;
            Surname = s.Surname;
            FirstAidQualified = s.FirstAidQualified;
            JobRole = s.JobRole;
            Email = s.Email;
            Password = s.Password;
            QualifiedSelectList = new List<SelectListItem> {
                new SelectListItem { Text = "Yes", Value = "yes" },
                new SelectListItem { Text = "No", Value = "no" } };
            ListOfJobRoles = new List<SelectListItem> {
                new SelectListItem { Text = "Manager", Value = "Manager" },
                new SelectListItem { Text = "Team Leader", Value = "Team Leader" },
                new SelectListItem { Text = "Team Member", Value = "Team Member" } };
        }

        public int StaffId { get; set; }

        [Required, DisplayName("Forename")]
        public string Forename { get; set; }

        [Required, DisplayName("Surname")]
        public string Surname { get; set; }

        [Required, DisplayName("Name")]
        public string FullName => Forename + " " + Surname;

        [Required, DisplayName("First Aid Qualified")]
        public bool FirstAidQualified { get; set; }

        [DisplayName("First Aid Qualified")]
        public string FirstAidQualifiedString => FirstAidQualified ? "Yes" : "No";

        [Required, DisplayName("Job Role")]
        public string JobRole { get; set; }

        [Required, DisplayName("Email Address")]
        public string Email { get; set; }

        [Required, DisplayName("Staff Password")]
        public string Password { get; set; }

        public List<SelectListItem> QualifiedSelectList { get; set; }

        public List<SelectListItem> ListOfJobRoles { get; set; }

        [Required, DisplayName("Is The Staff First Aid Qualified")]
        public string SelectedQualifiedValue { get; set; }
        public List<EventViewModel> Events { get; set; }

    }
}
