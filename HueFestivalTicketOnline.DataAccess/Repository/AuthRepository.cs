using HueFestivalTicketOnline.DataAccess.Repository.IRepository;
using HueFestivalTicketOnline.Models.DTOs.Authentiction;
using HueFestivalTicketOnline.Models.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HueFestivalTicketOnline.DataAccess.Repository
{
    public class AuthRepository: IAuthRepository
    {
        private readonly UserManager<Account> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public AuthRepository(UserManager<Account> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration) 
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<bool> CheckSeedRole()
        {
            bool isUserRoleExist = await _roleManager.RoleExistsAsync(StaticUserRole.USER);
            bool isAdminRoleExist = await _roleManager.RoleExistsAsync(StaticUserRole.ADMIN);
            if (isUserRoleExist && isAdminRoleExist)
            {
                return false;
            }

            await _roleManager.CreateAsync(new IdentityRole(StaticUserRole.USER));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRole.ADMIN));

            return true;
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
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim("JWTID", Guid.NewGuid().ToString())
                    };

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    var token = GenerateNewJWT(authClaims);
                    loginResult.Token = token;
                    return loginResult;
                }
                loginResult.CheckPassword = false;
                return loginResult;
            }
            loginResult.CheckUserName = false;
            return loginResult;
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

        private string GenerateNewJWT(List<Claim> authClaims)
        {
            var authSerect = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var tokenObject = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(2),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSerect, SecurityAlgorithms.HmacSha512Signature)
                );
            string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);
            return token;
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
