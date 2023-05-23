namespace HueFestivalTicketOnline.Models.DTOs
{
    public class FesTypeTicketDTO
    {
        public string TypeName { get; set; }
        public decimal Price { get; set; }
        public int Quanity { get; set; }
        public int FesProgramId { get; set; }
    }
}
