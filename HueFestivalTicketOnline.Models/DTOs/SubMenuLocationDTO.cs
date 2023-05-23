using Microsoft.AspNetCore.Http;

namespace HueFestivalTicketOnline.Models.DTOs
{
    public class SubMenuLocationDTO
    {
        public string CateName { get; set; }
        public int MenuLocationId { get; set; }
        public IFormFile File { get; set; }

    }
}
