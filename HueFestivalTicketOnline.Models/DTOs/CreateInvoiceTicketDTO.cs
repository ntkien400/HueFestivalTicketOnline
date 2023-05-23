using System.ComponentModel.DataAnnotations.Schema;

namespace HueFestivalTicketOnline.Models.DTOs
{
    public class CreateInvoiceTicketDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CCCD { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        [NotMapped]
        public int Quantity { get; set; }
        [NotMapped]
        public int FesTypeTicketId { get; set; }
    }
}
