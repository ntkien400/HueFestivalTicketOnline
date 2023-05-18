using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HueFestivalTicketOnline.DTOs
{
    public class ImageDTO
    {

        public int FesProgramId { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }
    }
}
