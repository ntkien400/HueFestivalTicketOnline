using Microsoft.AspNetCore.Http;

namespace HueFestivalTicketOnline.Models.DTOs
{
    public class CreateLocationDTO
    {
        public string? LocationName { get; set; }
        public string? Description { get; set; }
        public int? SubMenuLocationId { get; set; }
        public IFormFile? File { get; set; }
    }
}
