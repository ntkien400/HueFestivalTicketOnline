using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueFestivalTicketOnline.Models.Models
{
    public class News
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateChanged { get; set; }
        public string AccountId { get; set; }
        [ForeignKey("AccountId")]
        public Account Account { get; set; }
        
    }
}
