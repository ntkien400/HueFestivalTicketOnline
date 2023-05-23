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

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<TypeProgram>> GetTypeProgram(int id)
        {
            return await _unitOfWork.TypeProgram.GetAsync(id);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<TypeProgram>>> GetTypePrograms()
        {
            var objs = await _unitOfWork.TypeProgram.GetAllAsync();
            return Ok(objs);
        }

        [HttpPost]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task AddTypeProgram(TypeProgram typeProgram)
        {
            _unitOfWork.TypeProgram.Add(typeProgram);
            await _unitOfWork.SaveAsync();
        }

        [HttpPut]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task UpdateTypeProgram(TypeProgram typeProgram)
        {
            _unitOfWork.TypeProgram.Update(typeProgram);
            await _unitOfWork.SaveAsync();
        }

        [HttpDelete]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult> DeleteTypeProgram(int id)
        {
            var result = _unitOfWork.TypeProgram.Delete(id);
            if (result == true)
            {
                await _unitOfWork.SaveAsync();
                return Ok();
            }
            return BadRequest();
        }
    }
}
