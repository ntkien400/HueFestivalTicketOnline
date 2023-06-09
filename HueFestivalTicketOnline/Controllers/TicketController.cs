﻿using HueFestivalTicketOnline.DataAccess.Repository.IRepository;
using HueFestivalTicketOnline.Models.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using HueFestivalTicketOnline.Models.DTOs.Authentiction;
using HueFestivalTicketOnline.DataAccess.Repository.SendMailAndSms;
using System.Globalization;

namespace HueFestivalTicketOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISendEmail _sendEmail;
        public TicketController(IUnitOfWork unitOfWork, ISendEmail sendEmail)
        {
            _unitOfWork = unitOfWork;
            _sendEmail = sendEmail;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<Ticket>> GetTicket(int id)
        {
            var detailFes = await _unitOfWork.Ticket.GetAsync(id);
            if (detailFes == null)
            {
                return NotFound("Ticket not exists");
            }
            return Ok(detailFes);
        }

        [HttpGet]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<List<Ticket>>> GetTickets()
        {
            var objs = await _unitOfWork.Ticket.GetAllAsync(includesProperties: "FesProgram,Location");
            return Ok(objs);
        }

        [HttpPost]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult> CreateTicket(string invoiceId)
        {
            var invoice = await _unitOfWork.InvoiceTicket.GetFirstOrDefaultAsync(i => i.Id == Guid.Parse(invoiceId), includesProperties:"User,FesTypeTicket");
            var programDetail = await _unitOfWork.DetailFesLocation
                .GetFirstOrDefaultAsync(pd => pd.FesId == invoice.FesTypeTicket.FesProgramId, includesProperties: "FesProgram,Location");
            if(DateTime.Now.Date >= DateTime.Parse(programDetail.EndDate))
            {
                return BadRequest("Festival has ended");
            }

            //Ctreate ticket by quantity
            var listInfo = new List<string>();
            var listTicket = new List<Ticket>();
            for (int i = 0; i < invoice.Quantity; i++)
            {
                var ticket = new Ticket();
                ticket.TicketCode = _unitOfWork.Ticket.GenerateTicketCode(12);
                ticket.DateCreated = DateTime.Now;
                ticket.DateExpried = DateTime.Parse(programDetail.EndDate);
                ticket.TicketInfo = programDetail.FesProgram.ProgramName
                                    + "|" + programDetail.StartDate
                                    + "|" + programDetail.EndDate
                                    + "|" + programDetail.Time
                                    + "|" + invoice.FesTypeTicket.Price
                                    + "|" + ticket.TicketCode
                                    + "|" + invoice.FesTypeTicket.TypeName;
                ticket.FesTypeTicketId = invoice.FesTypeTicketId;
                ticket.UserId = invoice.UserId;
                listTicket.Add(ticket);
                listInfo.Add(ticket.TicketInfo);
            }
            _unitOfWork.Ticket.AddRange(listTicket);
            var result = await _unitOfWork.SaveAsync();
            if(result > 0)
            {
                await _sendEmail.SendEmailAsync(invoice.User.Email, listInfo);
                return Ok("Create ticket successfuly and sent to user email");
            }
            return BadRequest("Someting wrong when adding");
        }

        //[HttpPut]
        //public async Task<ActionResult<CreateTicketDTO>> UpdateTicket(CreateTicketDTO updateTicket, int id)
        //{
        //    var objFromDb = await _unitOfWork.Ticket.GetAsync(id);
        //    if (objFromDb != null)
        //    {
        //        _mapper.Map(updateTicket, objFromDb);
        //        _unitOfWork.Ticket.Update(objFromDb);
        //        await _unitOfWork.SaveAsync();
        //        return Ok(updateTicket);
        //    }
        //    return BadRequest("Không thể cập nhật.");

        //}

        [HttpDelete]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<Ticket>> DeleteTicket(int id)
        {
            var ticket = await _unitOfWork.Ticket.GetAsync(id);
            if (ticket != null)
            {
                _unitOfWork.Ticket.Delete(ticket);
                var result = await _unitOfWork.SaveAsync();
                if(result > 0)
                {
                    return Ok("Delete successfully");
                }
                return BadRequest("Someting wrong when deleting");
            }
            return NotFound("Can't find ticket to delete");
        }
    }
}
