using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueFestivalTicketOnline.Models.DTOs
{
    public class ResetPasswordDto
    {
        public string OTPCode { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
