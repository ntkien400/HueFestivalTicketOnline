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
    public class DetailFesLocationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DetailFesLocationController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<DetailFesLocation>> GetDetailFesLocation(int id)
        {
            var detailFes = await _unitOfWork.DetailFesLocation.GetAsync(id); 
            if(detailFes == null)
            {
                return NotFound("Data not exists");
            }
            return Ok(detailFes);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<DetailFesLocation>>> GetDetailFesLocations()
        {
            var objs = await _unitOfWork.DetailFesLocation.GetAllAsync(includesProperties:"FesProgram,Location");
            return Ok(objs);
        }

        [HttpPost]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<CreateDetailFesLocationDTO>> AddDetailFesLocation(CreateDetailFesLocationDTO createDetail)
        {
            var result = CheckDate(createDetail.StartDate, createDetail.EndDate);
            if(!result)
            {
                return BadRequest("Date invalid");
            }
            _unitOfWork.DetailFesLocation.Add(_mapper.Map<CreateDetailFesLocationDTO, DetailFesLocation>(createDetail));
            await _unitOfWork.SaveAsync();
            return Ok(createDetail);
        }

        [HttpPut]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<CreateDetailFesLocationDTO>> UpdateDetailFesLocation(CreateDetailFesLocationDTO updateDetail, int id)
        {
            var objFromDb = await _unitOfWork.DetailFesLocation.GetAsync(id);
            if(objFromDb != null)
            {
                _mapper.Map(updateDetail, objFromDb);
                _unitOfWork.DetailFesLocation.Update(objFromDb);
                await _unitOfWork.SaveAsync();
                return Ok(updateDetail);
            }
            return BadRequest("Something wrong when update");
            
        }

        [HttpDelete]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<DetailFesLocation>> DeleteDetailFesLocation(int id)
        {
            var result = _unitOfWork.DetailFesLocation.Delete(id);
            if(result == true)
            {
                await _unitOfWork.SaveAsync();
                return Ok();
            }
            return BadRequest("Something wrong when delete");
        }
        private bool CheckDate(string s1, string s2)
        {
            var date1 = DateOnly.ParseExact(s1,"dd-MM-yyyy");
            var date2 = DateOnly.ParseExact(s2, "dd-MM-yyyy");
            var now = DateTime.Now;
            if (date1 < DateOnly.FromDateTime(now))
                return false;
            if (date1 > date2)
                return false;
            return true;
        }
    }
}
