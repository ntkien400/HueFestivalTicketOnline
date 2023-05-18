using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HueFestivalTicketOnline.Models.Models
{
    public class InvoiceTicket
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        public int Quantity { get; set; }
        [Required]
        public decimal TotalAmount { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public int FesTypeTicketId { get; set; }
        [ForeignKey("FesTypeTicketId")]
        public FesTypeTicket FesTypeTicket { get; set; }
    }
}
