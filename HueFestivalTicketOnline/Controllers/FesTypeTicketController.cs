using AutoMapper;
using HueFestivalTicketOnline.DataAccess.Repository.IRepository;
using HueFestivalTicketOnline.DTOs;
using HueFestivalTicketOnline.DTOs.Authentiction;
using HueFestivalTicketOnline.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
            var objFromDb =  await _unitOfWork.FesTypeTicket.GetFirstOrDefaultAsync(o => o.Id == id, includesProperties: "FesProgram,TypeTicket");
            if(objFromDb != null)
            {
                return Ok(objFromDb);
            }
            return NotFound("Không tìm thấy dữ liệu");
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
            await _unitOfWork.SaveAsync();
            return Ok("Thêm thành công");
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
                await _unitOfWork.SaveAsync();
                return Ok("Cập nhật thành công");
            }
            return BadRequest("Lỗi cập nhật");
           
        }

        [HttpDelete]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult> DeleteProgramTypeTicket(int id)
        {
            var result = _unitOfWork.FesTypeTicket.Delete(id);
            if (result == true)
            {
                await _unitOfWork.SaveAsync();
                return Ok("Xoá thành công");
            }
            return BadRequest("Lỗi xoá");
        }
    }
}
