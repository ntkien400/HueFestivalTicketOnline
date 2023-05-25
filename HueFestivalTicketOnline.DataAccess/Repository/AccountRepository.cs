using HueFestivalTicketOnline.DataAccess.Data;
using HueFestivalTicketOnline.DataAccess.Repository.IRepository;
using HueFestivalTicketOnline.Models.DTOs;
using HueFestivalTicketOnline.Models.DTOs.Authentiction;
using HueFestivalTicketOnline.Models.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HueFestivalTicketOnline.DataAccess.Repository
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<Account> _userManager;
        private readonly IConfiguration _configuration;

        public AccountRepository(ApplicationDbContext dbContext, UserManager<Account> userManager, IConfiguration configuration) : base(dbContext)
        {
            _dbContext = dbContext;
            _userManager= userManager;
            _configuration = configuration;
        }

        public async Task<RegisterResult> Register(RegisterDTO registerDto)
        {
            var registerResult = new RegisterResult();
            var isExistUser = await _userManager.FindByNameAsync(registerDto.UserName);
            var isExistEmail = await _userManager.FindByEmailAsync(registerDto.Email);
            var validEmail = ValidateEmail(registerDto.Email);
            var validPhone = ValidatePhone(registerDto.PhoneNumber);

            if (isExistUser != null)
            {
                registerResult.isExistUser = true;
                return registerResult;
            }
            if (isExistEmail != null)
            {
                registerResult.isExistEmail = true;
                return registerResult;
            }
            if (!validEmail)
            {
                registerResult.validEmail = false;
                return registerResult;
            }
            if (!validPhone)
            {
                registerResult.validPhone = false;
                return registerResult;
            }

            Account newUser = new Account()
            {
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                PhoneNumber = registerDto.PhoneNumber,
                CCCD = registerDto.CCCD,
                FristName = registerDto.FirstName,
                LastName = registerDto.LastName,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var createUserResult = await _userManager.CreateAsync(newUser, registerDto.Password);
            if (!createUserResult.Succeeded)
            {
                var errors = "Create user fail beacause:";
                foreach (var error in createUserResult.Errors)
                {
                    errors += "#" + error.Description;
                }
                registerResult.CreateUserResult = false;
                registerResult.errors = errors;
            }
            if (newUser.UserName.Equals("admin"))
            {
                await _userManager.AddToRoleAsync(newUser, StaticUserRole.ADMIN);
            }
            await _userManager.AddToRoleAsync(newUser, StaticUserRole.USER);
            registerResult.CreateUserResult = true;
            return registerResult;
        }
        public async Task<LoginResult> Login(LoginDTO loginDto)
        {
            var loginResult = new LoginResult();
            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user != null)
            {
                loginResult.CheckUserName = true;
                var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                if (isPasswordCorrect)
                {
                    loginResult.CheckPassword = true;
                    RefreshTokenDTO refreshToken = await GenerateAccessToken(user);
                    loginResult.RefreshTokenDTO = refreshToken;
                    return loginResult;
                }
                loginResult.CheckPassword = false;
                return loginResult;
            }
            loginResult.CheckUserName = false;
            return loginResult;
        }
        public async Task<RefreshTokenDTO> GenerateAccessToken (Account account)
        {
            var userRoles = await _userManager.GetRolesAsync(account);
            var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, account.UserName),
                        new Claim(ClaimTypes.NameIdentifier, account.Id),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            var token = GenerateNewJWT(authClaims);
            var refresTokenDto = new RefreshTokenDTO
            {
                RefreshToken = (await GenerateRefreshToken(account.Id, token.Id)).Token,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            };
            return refresTokenDto;
        }

        public async Task<RefreshToken> GenerateRefreshToken(string accountId, string tokenId)
        {
            var refreshToken = new RefreshToken();
            var randomNumber = new byte[32];
            
            using(var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                randomNumberGenerator.GetBytes(randomNumber);
                refreshToken.Token = Convert.ToBase64String(randomNumber);
                refreshToken.DateCreated= DateTime.Now;
                refreshToken.DateExpried = DateTime.Now.AddMonths(1);
                refreshToken.AccountId = accountId;
                refreshToken.JwtId = tokenId;
            }

            await _dbContext.AddAsync(refreshToken);
            await _dbContext.SaveChangesAsync();
            return refreshToken;
        }

        public Account GetAccountFromAccessToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var key = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = _configuration["JWT:ValidAudience"],
                ValidIssuer = _configuration["JWT:ValidIssuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                RequireExpirationTime = false,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            };
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
            if(jwtSecurityToken != null && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512Signature, StringComparison.InvariantCultureIgnoreCase))
            {
                var accountName = principal.FindFirstValue(ClaimTypes.Name);
                var accountInfo = _dbContext.Accounts.FirstOrDefault(a => a.UserName == accountName);
                return accountInfo;
            }
            return null;
        }

        public async Task<bool> ValidateRefreshToken(Account account, string refreshToken)
        {
            var accountRefreshToken = await _dbContext.RefreshTokens
                .OrderByDescending(a => a.DateExpried)
                .FirstOrDefaultAsync(a => a.Token == refreshToken);
            if (accountRefreshToken != null 
                && accountRefreshToken.AccountId == account.Id 
                && accountRefreshToken.DateExpried > DateTime.Now)
            {
                return true;
            }
            return false;
        }
        public async Task ChangePassword(Account account, string newPassword)
        {
            var passwordHash = _userManager.PasswordHasher.HashPassword(account,newPassword);
            account.PasswordHash = passwordHash;
            await _userManager.UpdateAsync(account);
        }
        private JwtSecurityToken GenerateNewJWT(List<Claim> authClaims)
        {
            var authSerect = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var tokenObject = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddMinutes(30),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSerect, SecurityAlgorithms.HmacSha512Signature)
                );
            return tokenObject;
        }
        private bool ValidateEmail(string email)
        {
            var emailValidation = new EmailAddressAttribute();

            return emailValidation.IsValid(email);
        }
        private bool ValidatePhone(string phone)
        {
            if (phone.Length != 10)
            {
                return false;
            }
            var phoneValidation = new PhoneAttribute();

            return phoneValidation.IsValid(phone);
        }
    }
}
