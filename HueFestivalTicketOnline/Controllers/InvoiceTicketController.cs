using AutoMapper;
using HueFestivalTicketOnline.DataAccess.Repository.IRepository;
using HueFestivalTicketOnline.DTOs;
using HueFestivalTicketOnline.DTOs.Authentiction;
using HueFestivalTicketOnline.Models.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HueFestivalTicketOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceTicketController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public InvoiceTicketController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<InvoiceTicket>> GetInvoiceTicket(string id)
        {
            var detailFes = await _unitOfWork.InvoiceTicket.GetFirstOrDefaultAsync(d => d.Id.ToString().Equals(id), includesProperties: "User,FesTypeTicket"); 
            if(detailFes == null)
            {
                return NotFound("Không tìm thấy dữ liệu.");
            }
            return Ok(detailFes);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<InvoiceTicket>>> GetInvoiceTickets()
        {
            var objs = await _unitOfWork.InvoiceTicket.GetAllAsync(includesProperties:"FesProgram,Location");
            return Ok(objs);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<CreateInvoiceTicketDTO>> AddInvoiceTicket(CreateInvoiceTicketDTO createInvoiceTicketDto)
        {
            InvoiceTicket invoiceTicket = new InvoiceTicket();
            User user = new User();
            var fesTypeTicket = await _unitOfWork.FesTypeTicket.GetAsync(createInvoiceTicketDto.FesTypeTicketId);
            if(fesTypeTicket == null)
            {
                return BadRequest("Typeticket is wrong");
            }
            //Check ticket amount
            var resultamount = _unitOfWork.FesTypeTicket.CheckTicketAmount(fesTypeTicket);
            if (!resultamount)
            {
                return BadRequest("Ticket sold out");
            }
            var ticketprice = fesTypeTicket.Price;
            var userId = Guid.NewGuid().ToString();
            //Add customer
            user.Id = Guid.Parse(userId);
            _mapper.Map(createInvoiceTicketDto, user);
            _unitOfWork.User.Add(user);
            //Add invoice ticket
            invoiceTicket.Id = Guid.NewGuid();
            invoiceTicket.FesTypeTicketId = createInvoiceTicketDto.FesTypeTicketId;
            invoiceTicket.Quantity = createInvoiceTicketDto.Quantity;
            invoiceTicket.TotalAmount = ticketprice * createInvoiceTicketDto.Quantity;
            invoiceTicket.DateCreated = DateTime.Now;
            invoiceTicket.UserId = Guid.Parse(userId);
            _unitOfWork.InvoiceTicket.Add(invoiceTicket);
            //Update ticket amount
            bool result = _unitOfWork.FesTypeTicket.UpdateTicketAmount(fesTypeTicket, createInvoiceTicketDto.Quantity);
            if(!result)
            {
                return BadRequest("Quantity exceeds existing tickets");
            }

            await _unitOfWork.SaveAsync();
            return Ok("Mua vé thành công.");
        }

        [HttpPut]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<CreateInvoiceTicketDTO>> UpdateInvoiceTicket(CreateInvoiceTicketDTO updateDetail, int id)
        {
            var objFromDb = await _unitOfWork.InvoiceTicket.GetAsync(id);
            if(objFromDb != null)
            {
                _mapper.Map(updateDetail, objFromDb);
                _unitOfWork.InvoiceTicket.Update(objFromDb);
                await _unitOfWork.SaveAsync();
                return Ok(updateDetail);
            }
            return BadRequest("Không thể cập nhật.");
            
        }

        [HttpDelete]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<InvoiceTicket>> DeleteInvoiceTicket(int id)
        {
            var result = _unitOfWork.InvoiceTicket.Delete(id);
            if(result == true)
            {
                await _unitOfWork.SaveAsync();
                return Ok();
            }
            return BadRequest("Không thể xoá dữ liệu.");
        }
    }
}
