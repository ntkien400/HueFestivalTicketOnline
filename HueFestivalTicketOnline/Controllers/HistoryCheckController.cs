using HueFestivalTicketOnline.DataAccess.Repository.IRepository;
using HueFestivalTicketOnline.Models.DTOs.Authentiction;
using HueFestivalTicketOnline.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Security.Claims;

namespace HueFestivalTicketOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryCheckController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public HistoryCheckController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = StaticUserRole.ADMIN)]
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
        [Authorize(Roles =StaticUserRole.ADMIN)]
        public async Task<ActionResult<List<HistoryCheck>>> GetHistoryChecks(string? Date, int? programId)
        {
            var includes = new List<Expression<Func<HistoryCheck, bool>>>();
            
            if(Date != null)
            {
                includes.Add(x => x.DateChecked.Date.Date == DateTime.Parse(Date));
            }
            if(programId != null)
            {
                includes.Add(y => y.FesProgramId == programId);
            }
            var objs = await _unitOfWork.HistoryCheck.GetAllAsync(includes:includes);
            return Ok(objs);
        }

        [HttpPost]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult> InsertHistoryCheck(string ticketInfo, int programId)
        {
            var historyCheck = new HistoryCheck();
            var AccountId = HttpContext.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value;
            var listStr = ticketInfo.Split("|");
            var ticketCode = listStr[5];
            var ticket = await _unitOfWork.Ticket.GetFirstOrDefaultAsync(t => t.TicketCode == ticketCode, includesProperties:"FesTypeTicket");

            if(ticket != null)
            {
                if(ticketInfo != ticket.TicketInfo)
                {
                    historyCheck.AccountId = AccountId;
                    historyCheck.FesProgramId = programId;
                    historyCheck.DateChecked = DateTime.Now;
                    historyCheck.Status = false;
                    _unitOfWork.HistoryCheck.Add(historyCheck);
                    await _unitOfWork.SaveAsync();
                    return BadRequest("Ticket invalid");
                }
                if(ticket.DateExpried < DateTime.Now)
                {
                    historyCheck.AccountId = AccountId;
                    historyCheck.FesProgramId = programId;
                    historyCheck.DateChecked = DateTime.Now;
                    historyCheck.Status = false;
                    _unitOfWork.HistoryCheck.Add(historyCheck);
                    await _unitOfWork.SaveAsync(); 
                    return BadRequest("Ticket invalid");
                }
                if(ticket.FesTypeTicket.FesProgramId != programId)
                {
                    historyCheck.AccountId = AccountId;
                    historyCheck.FesProgramId = programId;
                    historyCheck.DateChecked = DateTime.Now;
                    historyCheck.Status = false;
                    _unitOfWork.HistoryCheck.Add(historyCheck);
                    await _unitOfWork.SaveAsync();
                    return BadRequest("Ticket invalid");
                }
                historyCheck.AccountId = AccountId;
                historyCheck.FesProgramId = programId;
                historyCheck.DateChecked = DateTime.Now;
                historyCheck.Status = true;
                _unitOfWork.HistoryCheck.Add(historyCheck);
                await _unitOfWork.SaveAsync();
                return Ok("Ticket valid");
                
            }
            else
            {
                return NotFound("Ticket not exists");
            }
            
        }

        //[HttpPut]
        //[Authorize(Roles = StaticUserRole.ADMIN)]
        //public async Task<ActionResult<CreateHistoryCheckDTO>> UpdateHistoryCheck(CreateHistoryCheckDTO updateDetail, int id)
        //{
        //    var objFromDb = await _unitOfWork.HistoryCheck.GetAsync(id);
        //    if (objFromDb != null)
        //    {
        //        _mapper.Map(updateDetail, objFromDb);
        //        _unitOfWork.HistoryCheck.Update(objFromDb);
        //        await _unitOfWork.SaveAsync();
        //        return Ok(updateDetail);
        //    }
        //    return BadRequest("Something wrong when update");

        //}

        [HttpDelete]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<HistoryCheck>> DeleteHistoryCheck(int id)
        {
            var history = await _unitOfWork.HistoryCheck.GetAsync(id);
            if (history != null)
            {
                _unitOfWork.HistoryCheck.Delete(history);
                var result = await _unitOfWork.SaveAsync();
                if(result > 0)
                {
                    return Ok("Delete successfully");
                }
                return BadRequest("Something wrong when deleting");
            }
            return NotFound("Can't find history to delete");
        }

    }
}
