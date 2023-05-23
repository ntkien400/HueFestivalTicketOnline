using Microsoft.AspNetCore.Identity;

namespace HueFestivalTicketOnline.Models.Models
{
    public class Account : IdentityUser
    {
        public string? PasswordOTP { get; set; }
        public DateTime PasswordOTPExpried { get; set; }
    }
}
