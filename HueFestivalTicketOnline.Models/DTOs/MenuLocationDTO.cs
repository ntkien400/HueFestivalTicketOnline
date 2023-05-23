namespace HueFestivalTicketOnline.Models.DTOs
{
    public class MenuLocationDTO
    {
        public int MenuId { get; set; }
        public string MenuTitle { get; set; }
        public ICollection<ViewSubMenuLocation> SubMenus { get; set; } = new List<ViewSubMenuLocation>(); 
    }
}
