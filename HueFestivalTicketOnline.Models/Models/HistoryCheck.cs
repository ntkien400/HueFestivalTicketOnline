using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueFestivalTicketOnline.Models.Models
{
    public class HistoryCheck
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime DateChecked { get; set; }
        [Required]
        public bool Status { get; set; }
        [Required]
        public string AccountId { get; set; }
        [ForeignKey("AccountId")]
        public Account Account { get; set; }
        public int FesProgramId { get; set; }
        [ForeignKey("FesProgramId")]
        public FesProgram FesProgram { get; set; }
    }
}
