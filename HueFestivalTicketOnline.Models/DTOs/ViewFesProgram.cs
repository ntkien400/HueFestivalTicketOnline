using HueFestivalTicketOnline.Models.Models;

namespace HueFestivalTicketOnline.Models.DTOs
{
    public class ViewFesProgram
    {
        public int ProgramId { get; set; }
        public string ProgramName { get; set; }
        public string Content { get; set; }
        public int TypeOfflineId { get; set; }
        public int TypeProgramId { get; set; }
        public DetailFesLocation DetailList { get; set; }
        public List<string> ImageUrls { get; set; } = new List<string>();
    }
}
