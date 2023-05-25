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

        public async Task<bool> MakeAdmin(UpdateRolesDTO updateRolesDto)
        {
            var user = await _userManager.FindByNameAsync(updateRolesDto.UserName);
            if (user == null)
            {
                return false;
            }
            await _userManager.AddToRoleAsync(user, StaticUserRole.ADMIN);
            return true;
        }

        public async Task<bool> RemoveAdmin(UpdateRolesDTO updateRolesDto)
        {
            var user = await _userManager.FindByNameAsync(updateRolesDto.UserName);
            if (user == null)
            {
                return false;
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                if (userRole.Equals(StaticUserRole.ADMIN))
                {
                    await _userManager.RemoveFromRoleAsync(user, StaticUserRole.ADMIN);
                    return true;
                }
            }
            return false;
        }
        

        
    }
}
