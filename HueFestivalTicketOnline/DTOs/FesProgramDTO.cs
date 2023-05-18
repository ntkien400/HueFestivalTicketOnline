using HueFestivalTicketOnline.Models.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HueFestivalTicketOnline.DTOs
{
    public class FesProgramDTO
    {
        public string ProgramName { get; set; }
        public string Content { get; set; }
        public int TypeOfflineId { get; set; }
        public int TypeProgramId { get; set; }
    }
}
