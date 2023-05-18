using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HueFestivalTicketOnline.Models.Models
{
    public class SubMenuLocation
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string CateName { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        public int MenuLocationId { get; set; }
        [ForeignKey("MenuLocationId")]
        public MenuLocation MenuLocation { get; set; }
    }
}