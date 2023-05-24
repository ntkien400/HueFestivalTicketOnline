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
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<InvoiceTicket>> GetInvoiceTicket(string id)
        {
            var detailFes = await _unitOfWork.InvoiceTicket.GetFirstOrDefaultAsync(d => d.Id.ToString() == id, includesProperties: "User,FesTypeTicket"); 
            if(detailFes == null)
            {
                return NotFound("Invoice is not exists");
            }
            return Ok(detailFes);
        }

        [HttpGet("get-all-invoices")]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<List<InvoiceTicket>>> GetInvoiceTickets()
        {
            var objs = await _unitOfWork.InvoiceTicket.GetAllAsync(includesProperties:"FesProgram,Location");
            return Ok(objs);
        }

        [HttpPost]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<CreateInvoiceTicketDTO>> AddInvoiceTicket(CreateInvoiceTicketDTO createInvoiceTicketDto)
        {
            InvoiceTicket invoiceTicket = new InvoiceTicket();
            User user = new User();
            var fesTypeTicket = await _unitOfWork.FesTypeTicket.GetAsync(createInvoiceTicketDto.FesTypeTicketId);
            if(fesTypeTicket == null)
            {
                return NotFound("Typeticket not exists");
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
            bool resultUpdateTicketAmount = _unitOfWork.FesTypeTicket.UpdateTicketAmount(fesTypeTicket, createInvoiceTicketDto.Quantity);
            if(!resultUpdateTicketAmount)
            {
                return BadRequest("Quantity exceeds existing tickets");
            }

            var result =  await _unitOfWork.SaveAsync();
            if(result > 0)
            {
                return Ok("Create ticket successfully");
            }
            return BadRequest("Something wrong when adding");
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
                var result = await _unitOfWork.SaveAsync();
                if(result > 0)
                {
                    return Ok(updateDetail);
                }
                return BadRequest("Something wrong when updating");
            }
            return NotFound("Invoice not exists");
            
        }

        [HttpDelete]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<InvoiceTicket>> DeleteInvoiceTicket(int id)
        {
            var invoice = await _unitOfWork.InvoiceTicket.GetAsync(id);
            if(invoice != null)
            {
                _unitOfWork.InvoiceTicket.Delete(invoice);
                var result = await _unitOfWork.SaveAsync();
                if(result > 0)
                {
                    return Ok("Delete successfully");
                }
                return BadRequest("Something wrong when deleting");
            }
            return NotFound("Invoice not exists");
        }
    }
}
