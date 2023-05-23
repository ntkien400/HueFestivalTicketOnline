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
            return NotFound("Không tìm thấy dữ liệu.");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<FesProgram>>> GetFesPrograms()
        {
            var objs = await _unitOfWork.FesProgram.GetAllAsync();
            return Ok(objs);
        }

        [HttpPost]
        [Authorize(Roles =StaticUserRole.ADMIN)]
        public async Task<ActionResult<FesProgram>> AddFesProgram(FesProgramDTO fesProgram)
        {
            _unitOfWork.FesProgram.Add(_mapper.Map<FesProgramDTO, FesProgram>(fesProgram));
            await _unitOfWork.SaveAsync();
            return Ok("Thêm thành công");
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
                await _unitOfWork.SaveAsync();
                return Ok(objFromDb);
            }
            return BadRequest("Không thể cập nhật.");
        }

        [HttpDelete]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<Location>> DeleteFesProgram(int id)
        {
            var result = _unitOfWork.FesProgram.Delete(id);
            if (result == true)
            {
                await _unitOfWork.SaveAsync();
                return Ok();
            }
            return BadRequest("Không thể xoá dữ liệu.");
        }
    }
}
