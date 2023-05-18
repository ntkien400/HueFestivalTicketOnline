
using HueFestivalTicketOnline.Models.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HueFestivalTicketOnline.DTOs
{
    public class ViewSubMenuLocation
    {
        public int Id { get; set; }
        public string CateName { get; set; }
        public int? ImageId { get; set; }
        public int MenuLocationId { get; set; }

    }
}
