using AutoMapper;
using HueFestivalTicketOnline.DataAccess.Repository.IRepository;
using HueFestivalTicketOnline.DTOs;
using HueFestivalTicketOnline.Models.Models;
using QRCoder;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System;
using System.Security.Cryptography;
using System.Text;

namespace HueFestivalTicketOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISendEmail _sendEmail;
        public TicketController(IUnitOfWork unitOfWork, IMapper mapper, ISendEmail sendEmail)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sendEmail = sendEmail;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetTicket(int id)
        {
            var detailFes = await _unitOfWork.Ticket.GetAsync(id);
            if (detailFes == null)
            {
                return NotFound("Không tìm thấy dữ liệu.");
            }
            return Ok(detailFes);
        }

        [HttpGet]
        public async Task<ActionResult<List<Ticket>>> GetTickets()
        {
            var objs = await _unitOfWork.Ticket.GetAllAsync(includesProperties: "FesProgram,Location");
            return Ok(objs);
        }

        [HttpPost]
        public async Task<ActionResult<CreateTicketDTO>> CreateTicket(CreateTicketDTO createTicketDto)
        {
            var fesTypeTicket = await _unitOfWork.FesTypeTicket.GetAsync(createTicketDto.FesTypeTicketId);
            if(fesTypeTicket == null)
            {
                return BadRequest("Type ticket invalid");
            }

            var program = await _unitOfWork.FesProgram.GetAsync(createTicketDto.FesProgramId);
            if (program == null)
            {
                return BadRequest("Festival program invalid");
            }

            var programDetail = await _unitOfWork.DetailFesLocation
                .GetFirstOrDefaultAsync(pd => pd.FesId == createTicketDto.FesProgramId, includesProperties: "FesProgram,Location");
            if(DateTime.Now >= DateTime.Parse(programDetail.EndDate))
            {
                return BadRequest("Programe has ended");
            }

            var ticket = new Ticket();
            for (int i = 0; i < createTicketDto.Quantity; i++)
            {
                ticket.TicketCode = GenerateTicketCode(12);
                ticket.DateCreated = DateTime.Now;
                ticket.DateExpried = DateTime.Parse(programDetail.EndDate);
                ticket.TicketInfo = programDetail.FesProgram.ProgramName
                                    + "\n" + programDetail.StartDate
                                    + " - " + programDetail.EndDate
                                    + "\n" + "VND" + fesTypeTicket.Price
                                    + "\n" + "Mã vé: " + ticket.TicketCode
                                    + "\n" + "Loại vé: " + fesTypeTicket.TypeName;
                _unitOfWork.Ticket.Add(ticket);
                ///await _unitOfWork.SaveAsync();
                await _sendEmail.SendEmailAsync(createTicketDto.UserEmail, ticket);

            }
            return Ok("Create ticket successfuly and sent to user email");
        }
/*
        [HttpPut]
        public async Task<ActionResult<CreateTicketDTO>> UpdateTicket(CreateTicketDTO updateTicket, int id)
        {
            var objFromDb = await _unitOfWork.Ticket.GetAsync(id);
            if (objFromDb != null)
            {
                _mapper.Map(updateTicket, objFromDb);
                _unitOfWork.Ticket.Update(objFromDb);
                await _unitOfWork.SaveAsync();
                return Ok(updateTicket);
            }
            return BadRequest("Không thể cập nhật.");

        }
*/
        [HttpDelete]
        public async Task<ActionResult<Ticket>> DeleteTicket(int id)
        {
            var result = _unitOfWork.Ticket.Delete(id);
            if (result == true)
            {
                await _unitOfWork.SaveAsync();
                return Ok();
            }
            return BadRequest("Không thể xoá dữ liệu.");
        }

        private string GenerateTicketCode(int length)
        {
            const string valid = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder sb = new StringBuilder();
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] uintBuffer = new byte[sizeof(uint)];

                while (length-- > 0)
                {
                    rng.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    sb.Append(valid[(int)(num % (uint)valid.Length)]);
                }
            }

            return sb.ToString();
        }

        
    }
}
