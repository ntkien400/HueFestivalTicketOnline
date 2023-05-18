using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueFestivalTicketOnline.Models.Models
{
    public class Location
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string LocationName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public int SubMenuLocationId { get; set; }
        [ForeignKey("SubMenuLocationId")]
        public SubMenuLocation SubMenuLocation { get; set; }
    }
}
