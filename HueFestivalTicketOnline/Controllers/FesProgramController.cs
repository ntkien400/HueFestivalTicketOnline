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
    public class FesProgramController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FesProgramController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ViewFesProgram>> GetFesProgram(int id)
        {
            var viewFes = new ViewFesProgram();
            var fes = await _unitOfWork.FesProgram.GetAsync(id);
            if(fes != null)
            {
                var images = await _unitOfWork.Image.GetAllAsync(i => i.FesProgramId == fes.Id);
                var detail = await _unitOfWork.DetailFesLocation.GetFirstOrDefaultAsync(d => d.FesId == id);
                viewFes = _mapper.Map<FesProgram, ViewFesProgram>(fes);
                foreach(var image in images)
                {
                    viewFes.ImageUrls.Add(image.ImageUrl);
                }
                viewFes.DetailList = detail;
                return Ok(viewFes);
            }
            return NotFound("Fes program not exists");
        }

        [HttpGet("get-all-fes-program")]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<List<FesProgram>>> GetFesPrograms()
        {
            var objs = await _unitOfWork.FesProgram.GetAllAsync();
            return Ok(objs);
        }

        [HttpGet("get-fes-program-list")]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<List<FesProgram>>> GetFesProgramsByTypeProgram(int typeId)
        {
            var fesPrograms = await _unitOfWork.FesProgram.GetAllAsync(f => f.TypeProgramId == typeId);
            return Ok(fesPrograms);
        }

        [HttpPost]
        [Authorize(Roles =StaticUserRole.ADMIN)]
        public async Task<ActionResult<FesProgram>> AddFesProgram(FesProgramDTO fesProgram)
        {
            var checkFieldNull = fesProgram.GetType().GetProperties()
                            .Select(f => f.GetValue(fesProgram))
                            .Any(value => value != null);
            if(!checkFieldNull)
            {
                _unitOfWork.FesProgram.Add(_mapper.Map<FesProgramDTO, FesProgram>(fesProgram));
                var result = await _unitOfWork.SaveAsync();
                if (result > 0)
                {
                    return Ok("Thêm thành công");
                }
                return BadRequest("Something wrong when adding");
            }
            return BadRequest("You must fill all field");
        }
        
        [HttpPut]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<FesProgram>> UpdateFesProgram(FesProgramDTO fesProgram, int id)
        {
            var objFromDb = await _unitOfWork.FesProgram.GetAsync(id);
            if(objFromDb != null)
            {
                _mapper.Map(fesProgram, objFromDb);
                _unitOfWork.FesProgram.Update(objFromDb);
                var result = await _unitOfWork.SaveAsync();
                if(result > 0)
                {
                    return Ok("Update successfully");
                }
                return BadRequest("Something wrong when updating");
            }
            return NotFound("Can't find fes program to update");
        }

        [HttpDelete]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<Location>> DeleteFesProgram(int id)
        {
            var fesProgram = await _unitOfWork.FesProgram.GetAsync(id);
            if (fesProgram != null)
            {
                _unitOfWork.FesProgram.Delete(fesProgram);
                var result = await _unitOfWork.SaveAsync();
                if (result > 0)
                {
                    return Ok("Delete successfully");
                }
                return BadRequest("Something wrong when deleting");
            }
            return NotFound("Can't find fes program to delete");
        }
    }
}
