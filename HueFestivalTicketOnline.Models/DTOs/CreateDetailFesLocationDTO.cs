namespace HueFestivalTicketOnline.Models.DTOs
{
    public class CreateDetailFesLocationDTO
    {
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? Time { get; set; }
        public decimal? Price { get; set; }
        public int? FesId { get; set; }
        public int? LocationId { get; set; }
    }
}
