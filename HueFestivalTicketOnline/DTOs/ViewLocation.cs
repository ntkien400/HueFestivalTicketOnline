using HueFestivalTicketOnline.Models.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HueFestivalTicketOnline.DTOs
{
    public class ViewLocation
    {
        public int Id { get; set; }
        public string LocationName { get; set; }
        public string Description { get; set; }
        public int CategroryLocationId { get; set; }
        public int ImageId { get; set; }
    }
}
