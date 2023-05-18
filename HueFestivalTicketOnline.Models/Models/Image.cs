using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HueFestivalTicketOnline.Models.Models
{
    public class Image
    {
        [Key]
        public Guid Id { get; set; }
        public string ImageUrl { get; set; }
        [Required]
        public int FesProgramId { get; set; }
        [ForeignKey("FesProgramId")]
        public FesProgram FesProgram { get; set; }
    }
}