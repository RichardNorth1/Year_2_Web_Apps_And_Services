using System.ComponentModel.DataAnnotations;

namespace ThAmCo.Catering.Data
{
    public class Menu
    {
        public Menu()
        {
        }

        public Menu(int menuId, string menuName)
        {
            MenuId = menuId;
            MenuName = menuName;
        }

        [Key]
        public int MenuId { get; set; }
        [Required,MaxLength(50)]
        public string MenuName { get; set; }
        public List<MenuFoodItem> MenuItems { get; set; }
    }
}
