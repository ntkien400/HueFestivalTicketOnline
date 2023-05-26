using AutoMapper;
using HueFestivalTicketOnline.DataAccess.Repository.IRepository;
using HueFestivalTicketOnline.Models.DTOs;
using HueFestivalTicketOnline.Models.DTOs.Authentiction;
using HueFestivalTicketOnline.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HueFestivalTicketOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FesTypeTicketController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FesTypeTicketController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<FesTypeTicket>> GetProgramTypeTicket(int id)
        {
            var objFromDb =  await _unitOfWork.FesTypeTicket.GetFirstOrDefaultAsync(o => o.Id == id, includesProperties: "FesProgram");
            if(objFromDb != null)
            {
                return Ok(objFromDb);
            }
            return NotFound("Type ticket not exists");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<FesTypeTicket>>> GetProgramTypeTickets()
        {
            var objs = await _unitOfWork.FesTypeTicket.GetAllAsync();
            return Ok(objs);
        }

        [HttpPost]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult> AddProgramTypeTicket(FesTypeTicketDTO programTypeTicketDto)
        {
            var programTypeTicket = new FesTypeTicket();
            _mapper.Map(programTypeTicketDto, programTypeTicket);
            _unitOfWork.FesTypeTicket.Add(programTypeTicket);
            var result = await _unitOfWork.SaveAsync();
            if(result > 0)
            {
                return Ok("Thêm thành công");
            }
            return BadRequest("Something wrong when adding");
        }

        [HttpPut]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult> UpdateProgramTypeTicket(FesTypeTicketDTO programTypeTicketDto, int id)
        {
            var objFromDb = await _unitOfWork.FesTypeTicket.GetAsync(id);
            if(objFromDb != null)
            {
                _mapper.Map(programTypeTicketDto, objFromDb);
                _unitOfWork.FesTypeTicket.Update(objFromDb);
                var result = await _unitOfWork.SaveAsync();
                if(result > 0)
                {
                    return Ok("Update successfully");
                }
                return BadRequest("Something wrong when updating");
            }
            return NotFound("can't find type ticket to update");
           
        }

        [HttpDelete]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult> DeleteFesTypeTicket(int id)
        {
            var fesTypeTicket = await _unitOfWork.FesTypeTicket.GetAsync(id);
            if (fesTypeTicket != null)
            {
                _unitOfWork.FesTypeTicket.Delete(fesTypeTicket);
                var result = await _unitOfWork.SaveAsync();
                if (result > 0)
                {
                    return Ok("Delete successfully");
                }
                return BadRequest("Something wrong when deleting");
            }
            return NotFound("Can't find fes type ticket to delete");
        }
    }
}
