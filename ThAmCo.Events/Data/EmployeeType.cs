using System.ComponentModel;

namespace ThAmCo.Events.Data
{
    public enum EmployeeType
    {
        [Description("Manager")]
        MANAGER ,

        [Description("Team Leader")]
        TEAMLEADER,

        [Description("Team Member")]
        TEAMMEMBER

    }
}