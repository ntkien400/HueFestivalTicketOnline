using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HueFestivalTicketOnline.Models.Models
{
    public class DetailFesLocation
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string StartDate { get; set; }
        [Required]
        public string EndDate { get; set; }
        [Required]
        public string Time { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int FesId { get; set; }
        [ForeignKey("FesId")]
        [JsonIgnore]
        public FesProgram FesProgram { get; set; }
        [Required]
        public int LocationId { get; set; }
        [ForeignKey("LocationId")]
        [JsonIgnore]
        public Location Location { get; set; }
    }
}
