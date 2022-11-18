using System.ComponentModel.DataAnnotations;
using ThAmCo.Catering.Data;

namespace ThAmCo.Catering.Models
{
    public class MenuDto
    {


        [Key]
        public int MenuId { get; set; }
        [Required, MaxLength(50)]
        public string MenuName { get; set; }
        public List<MenuFoodItem> MenuItems { get; set; }

    }
}
