using HueFestivalTicketOnline.Models.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace HueFestivalTicketOnline.Models.DTOs
{
    public class CreateNewsDTO
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public IFormFile? File { get; set; }
    }
}
