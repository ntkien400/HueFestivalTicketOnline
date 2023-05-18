using System.ComponentModel.DataAnnotations;

namespace HueFestivalTicketOnline.Models.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string CCCD { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, Phone]
        public string PhoneNumber { get; set; }
    }
}
