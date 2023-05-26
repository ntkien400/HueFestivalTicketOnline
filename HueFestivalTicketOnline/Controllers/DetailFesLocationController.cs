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

        [HttpGet("get-detail-fes-program-by-fes-program")]
        [AllowAnonymous]
        public async Task<ActionResult<DetailFesLocation>> GetDetailFesLocationByFesProgram(int fesProgramId)
        {
            var detailFes = await _unitOfWork.DetailFesLocation.GetFirstOrDefaultAsync(d => d.FesId == fesProgramId);
            if (detailFes == null)
            {
                return NotFound("Data not exists");
            }
            return Ok(detailFes);
        }

        [HttpGet]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<List<DetailFesLocation>>> GetDetailFesLocations()
        {
            var objs = await _unitOfWork.DetailFesLocation.GetAllAsync(includesProperties:"FesProgram,Location");
            return Ok(objs);
        }

        [HttpPost]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<CreateDetailFesLocationDTO>> AddDetailFesLocation(CreateDetailFesLocationDTO createDetail)
        {
            var checkFieldNotNull = createDetail.GetType().GetProperties()
                                .Select(crd => crd.GetValue(createDetail))
                                .Any(value => value != null);
            if(checkFieldNotNull)
            {
                var checkDate = _unitOfWork.DetailFesLocation.CheckDate(createDetail.StartDate, createDetail.EndDate);
                if (!checkDate)
                {
                    return BadRequest("Date invalid");
                }
                _unitOfWork.DetailFesLocation.Add(_mapper.Map<CreateDetailFesLocationDTO, DetailFesLocation>(createDetail));
                var result = await _unitOfWork.SaveAsync();
                if(result > 0)
                {
                    return Ok(createDetail);
                }
                return BadRequest("Something wrong when adding");
            }
            return BadRequest("You must fill all field");
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
                var result = await _unitOfWork.SaveAsync();
                if (result > 0)
                {
                    return Ok("Update successfully");
                }
                return BadRequest("Something wrong when updating");
            }
            return NotFound("Can't find to update");
            
        }

        [HttpDelete]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<DetailFesLocation>> DeleteDetailFesLocation(int id)
        {
            var detailFesLocation = await _unitOfWork.DetailFesLocation.GetAsync(id);
            if(detailFesLocation != null)
            {
                var result = await _unitOfWork.SaveAsync();
                if (result > 0)
                {
                    return Ok("Delete successfully");
                }
                return BadRequest("Something wrong when deleting");
            }
            return NotFound("Can't find to delete");
        }

        
    }
}
