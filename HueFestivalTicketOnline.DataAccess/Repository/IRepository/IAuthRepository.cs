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
        public Task<bool> MakeAdmin(UpdateRolesDTO updateRolesDto);
        public Task<bool> RemoveAdmin(UpdateRolesDTO updateRolesDto);
    }
}
