using HueFestivalTicketOnline.Models.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace HueFestivalTicketOnline.DTOs
{
    public class CreateTicketDTO
    {
        public string UserEmail { get; set; }
        public int FesTypeTicketId { get; set; }
        public int FesProgramId { get; set; }
        public int Quantity { get; set; }
    }
}
