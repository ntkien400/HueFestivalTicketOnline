﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueFestivalTicketOnline.Models.DTOs
{
    public class RefreshTokenDTO
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? Expiration { get; set; }
    }
}
