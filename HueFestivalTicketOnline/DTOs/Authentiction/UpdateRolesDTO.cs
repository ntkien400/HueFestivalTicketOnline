using System.ComponentModel.DataAnnotations;

namespace HueFestivalTicketOnline.DTOs.Authentiction
{
    public class UpdateRolesDTO
    {
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }
    }
}
