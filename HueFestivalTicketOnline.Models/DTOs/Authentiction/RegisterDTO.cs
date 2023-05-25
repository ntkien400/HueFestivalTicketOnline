using System.ComponentModel.DataAnnotations;

namespace HueFestivalTicketOnline.Models.DTOs.Authentiction
{
    public class RegisterDTO
    {
        [Required(ErrorMessage ="Username is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [EmailAddress, Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Phone, Required(ErrorMessage = "Phone is required")]
        public string PhoneNumber { get; set; }
        public string CCCD { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
