using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueFestivalTicketOnline.Models.Models
{
    public class AboutInformation
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string AboutTitle { get; set; }
        [Required]
        public string AboutContent { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        public DateTime DateChanged { get; set; }
        public string AccountId { get; set; }
        [ForeignKey("AccountId")]
        public Account Account { get; set; }
    }
}
