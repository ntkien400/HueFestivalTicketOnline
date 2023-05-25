using HueFestivalTicketOnline.Models.DTOs;
using HueFestivalTicketOnline.Models.DTOs.Authentiction;
using HueFestivalTicketOnline.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueFestivalTicketOnline.DataAccess.Repository.IRepository
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        public Task<RegisterResult> Register(RegisterDTO registerDto);
        public Task<LoginResult> Login(LoginDTO loginDto);
        public Task<RefreshTokenDTO> GenerateAccessToken(Account account);
        public Account GetAccountFromAccessToken(string token);
        public Task<bool> ValidateRefreshToken(Account account, string refreshToken);
        public Task ChangePassword(Account account, string newPassword);
    }
}
