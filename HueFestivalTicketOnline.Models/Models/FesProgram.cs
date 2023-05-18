using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueFestivalTicketOnline.Models.Models
{
    public class FesProgram
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ProgramName { get; set; }
        public string Content { get; set; }
        [Required]
        public int TypeOfflineId { get; set; }
        [ForeignKey("TypeOfflineId")]
        public TypeOffline TypeOffline { get; set; }
        [Required]
        public int TypeProgramId { get; set; }
        [ForeignKey("TypeProgramId")]
        public TypeProgram TypeProgram { get; set; }
    }
}
