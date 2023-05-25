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

        
        [HttpPost]
        [Route("make-admin")]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult> MakeAdmin(UpdateRolesDTO updateRolesDto)
        {
            var result = await _accountRepo.MakeAdmin(updateRolesDto);
            if(!result)
            {
                return BadRequest("Can't find the user");
            }
            return Ok("Now user is an Admin");
        }

        [HttpPost]
        [Route("remove-admin")]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult> RemoveAdmin(UpdateRolesDTO updateRolesDto)
        {
            var result = await _accountRepo.RemoveAdmin(updateRolesDto);
            if(!result)
            {
                return BadRequest("Can't find user or user is not Admin");
            }
            return Ok("User is not Admin anymore");

        }

    }

}
