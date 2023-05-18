using System.ComponentModel.DataAnnotations;

namespace HueFestivalTicketOnline.Models.Models
{
    public class TypeProgram
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string TypeName { get; set; }
    }
}