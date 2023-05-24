using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace HueFestivalTicketOnline.Models.DTOs
{
    public class ImageDTO
    {

        public int? FesProgramId { get; set; }
        public IFormFile? File { get; set; }
    }
}
