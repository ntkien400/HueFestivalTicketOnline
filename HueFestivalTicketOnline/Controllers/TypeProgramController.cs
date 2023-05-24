using HueFestivalTicketOnline.DataAccess.Repository.IRepository;
using HueFestivalTicketOnline.Models.DTOs.Authentiction;
using HueFestivalTicketOnline.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HueFestivalTicketOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeProgramController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public TypeProgramController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("get-type")]
        [AllowAnonymous]
        public async Task<ActionResult<TypeProgram>> GetTypeProgram(int id)
        {
            var typeProgram = await _unitOfWork.TypeProgram.GetAsync(id);
            if(typeProgram !=null)
            {
                return Ok(typeProgram);
            }
            return NotFound("Type program not exists");
        }

        [HttpGet("get-all-type")]
        [AllowAnonymous]
        public async Task<ActionResult<List<TypeProgram>>> GetTypePrograms()
        {
            var objs = await _unitOfWork.TypeProgram.GetAllAsync();
            return Ok(objs);
        }

        [HttpPost]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<TypeProgram>> AddTypeProgram([FromForm]TypeProgram typeProgram)
        {
            _unitOfWork.TypeProgram.Add(typeProgram);
            var result = await _unitOfWork.SaveAsync();
            if (result > 0)
            {
                return Ok(typeProgram);
            }
            return BadRequest("Something wrong when adding");
        }

        [HttpPut]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult> UpdateTypeProgram([FromForm] TypeProgram typeProgram)
        {
            var type = await _unitOfWork.TypeProgram.GetAsync(typeProgram.Id);
            if(type != null)
            {
                type.TypeName = typeProgram.TypeName;
                _unitOfWork.TypeProgram.Update(type);
                var result = await _unitOfWork.SaveAsync();
                if (result > 0)
                {
                    return Ok("Update successfully");
                }
                return BadRequest("Something wrong when updating");
            }
            return NotFound("Can't find type program to update");
        }

        [HttpDelete]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult> DeleteTypeProgram(int id)
        {
            var typeProgram = await _unitOfWork.TypeProgram.GetAsync(id);
            if (typeProgram != null)
            {
                _unitOfWork.TypeProgram.Delete(typeProgram);
                var result = await _unitOfWork.SaveAsync();
                if (result > 0)
                {
                    return Ok("Delete successfully");
                }
                return BadRequest("Something wrong when deleting");
            }
            return NotFound("Can't find type program to delete");
        }
    }
}
