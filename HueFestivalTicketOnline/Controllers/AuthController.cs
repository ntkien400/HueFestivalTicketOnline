using AutoMapper;
using HueFestivalTicketOnline.DTOs.Authentiction;
using HueFestivalTicketOnline.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace HueFestivalTicketOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<Account> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthController(UserManager<Account> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("seed-roles")]
        public async Task<ActionResult> SeedRoles()
        {
            bool isUserRoleExist = await _roleManager.RoleExistsAsync(StaticUserRole.USER);
            bool isAdminRoleExist = await _roleManager.RoleExistsAsync(StaticUserRole.ADMIN);
            if(isUserRoleExist && isAdminRoleExist)
            {
                return Ok("Seeding roles is already done.");
            }

            await _roleManager.CreateAsync(new IdentityRole(StaticUserRole.USER));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRole.ADMIN));

            return Ok("Seeding roles successfully!");
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            var isExistUser = await _userManager.FindByNameAsync(registerDto.UserName);
            var isExistEmail = await _userManager.FindByEmailAsync(registerDto.Email);
            var validEmail = ValidateEmail(registerDto.Email);
            var validPhone = ValidatePhone(registerDto.Phone);
            
            if(isExistUser != null)
            {
                return BadRequest("Username is already exist.");
            }
            if (isExistEmail != null)
            {
                return BadRequest("Email had been used.");
            }
            if(!validEmail)
            {
                return BadRequest("Email is invalid.");
            }
            if (!validPhone)
            {
                return BadRequest("Phone number is invalid.");
            }

            Account newUser = new Account()
            {
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var createUserResult = await _userManager.CreateAsync(newUser, registerDto.Password);
            if(!createUserResult.Succeeded)
            {
                var errors = "Create user fail beacause:";
                foreach(var error in createUserResult.Errors)
                {
                    errors += "#" + error.Description;
                }
                return BadRequest(errors);
            }
            if(newUser.UserName.Equals("admin"))
            {
                await _userManager.AddToRoleAsync(newUser, StaticUserRole.ADMIN);
            }
            await _userManager.AddToRoleAsync(newUser, StaticUserRole.USER);
            return Ok("Create user successfully");
        }
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login([FromBody]LoginDTO loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if(user == null)
            {
                return Unauthorized("Username is wrong");
            }

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if(!isPasswordCorrect)
            {
                return Unauthorized("Password is wrong");
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("JWTID", Guid.NewGuid().ToString())
            };

            foreach(var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = GenerateNewJWT(authClaims);
            return Ok(token);
        }

        [HttpPost]
        [Route("make-admin")]
        [Authorize(Roles =StaticUserRole.ADMIN)]
        public async Task<ActionResult> MakeAdmin(UpdateRolesDTO updateRolesDto)
        {
            var user = await _userManager.FindByNameAsync(updateRolesDto.UserName);
            if (user == null)
            {
                return Unauthorized("Username is wrong");
            }
            await _userManager.AddToRoleAsync(user, StaticUserRole.ADMIN);
            return Ok("User is now an Admin");
        }
        [HttpPost]
        [Route("remove-admin")]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult> RemoveAdmin(UpdateRolesDTO updateRolesDto)
        {
            var user = await _userManager.FindByNameAsync(updateRolesDto.UserName);
            if (user == null)
            {
                return Unauthorized("Username is wrong");
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach(var userRole in userRoles)
            {
                if (userRole.Equals(StaticUserRole.ADMIN))
                {
                    await _userManager.RemoveFromRoleAsync(user, StaticUserRole.ADMIN);
                    return Ok("User is no longer an Admin");
                }
            }
            return BadRequest("User is not Admin");
            
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
            if(phone.Length != 10)
            {
                return false;
            }
            var phoneValidation = new PhoneAttribute();

            return phoneValidation.IsValid(phone);
        }
    }

}
