using HueFestivalTicketOnline.Models.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HueFestivalTicketOnline.DTOs
{
    public class CreateLocationDTO
    {
        public string? LocationName { get; set; }
        public string? Description { get; set; }
        public int? SubMenuLocationId { get; set; }
        public IFormFile? File { get; set; }
    }
}
