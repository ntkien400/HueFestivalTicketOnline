using HueFestivalTicketOnline.DataAccess.Repository.IRepository;
using HueFestivalTicketOnline.Models.DTOs.Authentiction;
using HueFestivalTicketOnline.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HueFestivalTicketOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeOfflineController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public TypeOfflineController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<TypeOffline>> GetTypeOffline(int id)
        {
            var objFromDb =  await _unitOfWork.TypeOffline.GetAsync(id);
            if(objFromDb != null)
            {
                return Ok(objFromDb);
            }
            return NotFound("Type offline is not exists");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<TypeOffline>>> GetTypeOfflines()
        {
            var objs = await _unitOfWork.TypeOffline.GetAllAsync();
            return Ok(objs);
        }

        [HttpPost]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult> AddTypeOffline(TypeOffline typeOffline)
        {
            _unitOfWork.TypeOffline.Add(typeOffline);
            var result = await _unitOfWork.SaveAsync();
            if (result > 0)
            {
                return Ok(typeOffline);
            }
            return BadRequest("Something wrong when adding");
        }

        [HttpPut]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult> UpdateTypeOffline(TypeOffline typeOffline)
        {
            var objFromDb = await _unitOfWork.TypeOffline.GetAsync(typeOffline.Id);
            if(objFromDb != null)
            {
                objFromDb.TypeName = typeOffline.TypeName;
                _unitOfWork.TypeOffline.Update(objFromDb);
                var result = await _unitOfWork.SaveAsync();
                if (result > 0)
                {
                    return Ok("Update successfully");
                }
                return BadRequest("Something wrong when updating");
            }
            return NotFound("Can't find type offline to upadte");
           
        }

        [HttpDelete]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult> DeleteTypeOffline(int id)
        {
            var typeOffine = await _unitOfWork.TypeOffline.GetAsync(id);
            if (typeOffine != null)
            {
                _unitOfWork.TypeOffline.Delete(typeOffine);
                var result = await _unitOfWork.SaveAsync();
                if (result > 0)
                {
                    return Ok("Delete successfully");
                }
                return BadRequest("Something wrong when deleting");
            }
            return NotFound("Can't find type offline to delete");
        }
    }
}
