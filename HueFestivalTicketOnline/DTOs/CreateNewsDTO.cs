using HueFestivalTicketOnline.Models.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace HueFestivalTicketOnline.DTOs
{
    public class CreateNewsDTO
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public IFormFile? File { get; set; }
    }
}
