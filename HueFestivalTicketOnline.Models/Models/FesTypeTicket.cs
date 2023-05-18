using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueFestivalTicketOnline.Models.Models
{
    public class FesTypeTicket
    {
        public int Id { get; set; }
        [Required]
        public string TypeName { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Quanity { get; set; }
        public int FesProgramId { get; set; }
        [ForeignKey("FesProgramId")]
        public FesProgram FesProgram { get; set; }
    }
}
