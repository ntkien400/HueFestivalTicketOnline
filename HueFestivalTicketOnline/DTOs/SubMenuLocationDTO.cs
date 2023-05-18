
using HueFestivalTicketOnline.Models.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HueFestivalTicketOnline.DTOs
{
    public class SubMenuLocationDTO
    {
        public string CateName { get; set; }
        public int MenuLocationId { get; set; }
        public IFormFile File { get; set; }

    }
}
