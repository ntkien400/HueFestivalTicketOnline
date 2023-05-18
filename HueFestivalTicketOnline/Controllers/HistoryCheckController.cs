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
    public class HistoryCheckController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HistoryCheckController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<HistoryCheck>> GetHistoryCheck(int id)
        {
            var detailFes = await _unitOfWork.HistoryCheck.GetAsync(id); 
            if(detailFes == null)
            {
                return NotFound("Data not exists");
            }
            return Ok(detailFes);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<HistoryCheck>>> GetHistoryChecks()
        {
            var objs = await _unitOfWork.HistoryCheck.GetAllAsync(includesProperties:"FesProgram,Location");
            return Ok(objs);
        }

        [HttpPost]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult> InsertHistoryCheck(string ticketInfo)
        {
            var listStr = ticketInfo.Split("\n");
            var ticketCode = listStr[5].Substring(7,12);
            var ticket = await _unitOfWork.Ticket.GetFirstOrDefaultAsync(t => t.TicketCode == ticketCode);
            if(ticket != null)
            {
                if(ticket.DateExpried < DateTime.Now)
                {
                    
                    return BadRequest("Ticket is expired");
                }
            }
        }

        [HttpPut]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<CreateHistoryCheckDTO>> UpdateHistoryCheck(CreateHistoryCheckDTO updateDetail, int id)
        {
            var objFromDb = await _unitOfWork.HistoryCheck.GetAsync(id);
            if(objFromDb != null)
            {
                _mapper.Map(updateDetail, objFromDb);
                _unitOfWork.HistoryCheck.Update(objFromDb);
                await _unitOfWork.SaveAsync();
                return Ok(updateDetail);
            }
            return BadRequest("Something wrong when update");
            
        }

        [HttpDelete]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<HistoryCheck>> DeleteHistoryCheck(int id)
        {
            var result = _unitOfWork.HistoryCheck.Delete(id);
            if(result == true)
            {
                await _unitOfWork.SaveAsync();
                return Ok();
            }
            return BadRequest("Something wrong when delete");
        }
        
    }
}
