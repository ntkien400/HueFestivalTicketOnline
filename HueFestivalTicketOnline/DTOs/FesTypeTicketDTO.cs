using HueFestivalTicketOnline.Models.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HueFestivalTicketOnline.DTOs
{
    public class FesTypeTicketDTO
    {
        public string TypeName { get; set; }
        public decimal Price { get; set; }
        public int Quanity { get; set; }
        public int FesProgramId { get; set; }
    }
}
