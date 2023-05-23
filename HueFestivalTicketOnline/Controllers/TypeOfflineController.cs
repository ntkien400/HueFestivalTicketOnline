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
            return NotFound("Không tìm thấy dữ liệu");
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
            await _unitOfWork.SaveAsync();
            return Ok("Thêm thành công");
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
                await _unitOfWork.SaveAsync();
                return Ok("Cập nhật thành công");
            }
            return BadRequest("Lỗi cập nhật");
           
        }

        [HttpDelete]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult> DeleteTypeOffline(int id)
        {
            var result = _unitOfWork.TypeOffline.Delete(id);
            if (result == true)
            {
                await _unitOfWork.SaveAsync();
                return Ok("Xoá thành công");
            }
            return BadRequest("Lỗi xoá");
        }
    }
}
