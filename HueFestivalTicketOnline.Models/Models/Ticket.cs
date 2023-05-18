using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueFestivalTicketOnline.Models.Models
{
    public class Ticket
    {
        [Key]
        [Required, StringLength(12)]
        public string TicketCode { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        [Required]
        public DateTime DateExpried { get; set; }
        [Required]
        public string TicketInfo { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public int FesTypeTicketId { get; set; }
        [ForeignKey("FesTypeTicketId")]
        public FesTypeTicket FesTypeTicket { get; set; }
    }
}
