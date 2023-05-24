using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HueFestivalTicketOnline.Models.Models
{
    [Index(nameof(CCCD), IsUnique = true)]
    public class Account : IdentityUser
    {
        [Required]
        public string FristName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string CCCD { get; set; }
        public string? PasswordOTP { get; set; }
        public DateTime PasswordOTPExpried { get; set; }
    }
}
