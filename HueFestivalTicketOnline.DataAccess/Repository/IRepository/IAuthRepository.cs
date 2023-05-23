using HueFestivalTicketOnline.Models.DTOs.Authentiction;
using HueFestivalTicketOnline.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueFestivalTicketOnline.DataAccess.Repository.IRepository
{
    public interface IAuthRepository 
    {
        Task<bool> CheckSeedRole();
        Task<RegisterResult> Register(RegisterDTO registerDto);
        Task<LoginResult> Login(LoginDTO loginDto);
    }
}
