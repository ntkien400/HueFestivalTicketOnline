using HueFestivalTicketOnline.DataAccess.Repository.IRepository;
using HueFestivalTicketOnline.Models.DTOs.Authentiction;
using HueFestivalTicketOnline.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HueFestivalTicketOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _accountRepo;
        public AuthController(IAuthRepository accountRepo)
        {
            _accountRepo = accountRepo;
        }

        [HttpPost("seed-roles")]
        [AllowAnonymous]
        public async Task<ActionResult> SeedRoles()
        {
            var result = await _accountRepo.CheckSeedRole();
            if (result == true)
                return Ok("Seeding is aldready done");
            else
                return Ok("Seeding is successful");
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            var registerResult = await _accountRepo.Register(registerDto);
            if(registerResult.isExistEmail == true)
            {
                return BadRequest("Email is already used");
            }
            if(registerResult.isExistUser == true)
            {
                return BadRequest("Username is already used");
            }
            if (registerResult.validEmail == false)
            {
                return BadRequest("Email invalid");
            }
            if (registerResult.validPhone == false)
            {
                return BadRequest("Phone invalid");
            }
            if(registerResult.CreateUserResult == false)
            {
                return BadRequest(registerResult.errors);
            }
            return Ok("Account create successfully");

        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody]LoginDTO loginDto)
        {
            var loginResult = await _accountRepo.Login(loginDto);
            if(loginResult.CheckUserName == false)
            {
                return BadRequest("Username is wrong");
            }
            if(loginResult.CheckPassword == false)
            {
                return BadRequest("Password is wrong");
            }
            return Ok(loginResult.Token);
        }
        
    }

}
