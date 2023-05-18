using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueFestivalTicketOnline.Models.Models
{
    public class MenuLocation
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string MenuTitle { get; set; }
    }
}
