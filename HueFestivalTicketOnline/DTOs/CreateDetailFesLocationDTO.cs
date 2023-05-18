using HueFestivalTicketOnline.Models.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace HueFestivalTicketOnline.DTOs
{
    public class CreateDetailFesLocationDTO
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Time { get; set; }
        public decimal Price { get; set; }
        public int FesId { get; set; }
        public int LocationId { get; set; }
    }
}
