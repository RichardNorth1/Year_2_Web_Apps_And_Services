namespace ThAmCo.Events.DTOs
{
    public class MenuDto
    {
        public MenuDto()
        {
        }

        public MenuDto(MenuDto m)
        {
            MenuId = m.MenuId;
            MenuName = m.MenuName;
        }

        public int MenuId { get; set; }

        public string MenuName { get; set; }
    }
}
