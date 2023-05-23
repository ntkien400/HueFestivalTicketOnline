using System.ComponentModel.DataAnnotations;

namespace HueFestivalTicketOnline.Models.DTOs.Authentiction
{
    public class UpdateRolesDTO
    {
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }
    }
}
