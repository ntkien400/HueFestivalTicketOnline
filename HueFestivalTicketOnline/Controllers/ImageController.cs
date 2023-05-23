using AutoMapper;
using HueFestivalTicketOnline.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HueFestivalTicketOnline.Models.Models;
using HueFestivalTicketOnline.Models.DTOs.Authentiction;
using HueFestivalTicketOnline.Models.DTOs;

namespace HueFestivalTicketOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ImageController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetImage(int id)
        {
            var objFromDb =  await _unitOfWork.Image.GetAsync(id);
            if (objFromDb != null)
            {
                return Ok(objFromDb);
            }
            return NotFound("Không tìm thấy dữ liệu");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> GetImagesByProgramId(int id)
        {
            var objs = await _unitOfWork.Image.GetAllAsync(o => o.FesProgramId == id);
            if(objs !=null)
            {
                return Ok(objs);
            }
            return NotFound("Không tìm thấy lễ hội bạn yêu cầu");
        }
        /*[HttpGet("{fesName}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Image>>> GetImagesByProgramName(string fesName)
        {
            var fesProgram = await _unitOfWork.FesProgram.GetFirstOrDefaultAsync(f =>
                f.ProgramName.Trim().ToLower().Equals(fesName.Trim().ToLower()));  
            if(fesProgram !=null)
            {
                var objs = await _unitOfWork.Image.GetAllAsync(o => o.FesProgramId == fesProgram.Id);
                return Ok(objs);
            }
            return NotFound("Không tìm lễ hội bạn yêu cầu");
        }*/
        [HttpPost]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult> AddImage([FromForm]ImageDTO imageDto)
        {
            if(imageDto.File != null)
            {
                var image = new Image();
                string fileName = Guid.NewGuid().ToString();
                var extension = Path.GetExtension(imageDto.File.FileName);

                using (var fileStream = new FileStream(
                    Path.Combine(@"images", fileName + extension),
                    FileMode.Create))
                {
                    imageDto.File.CopyTo(fileStream);
                }
                image.ImageUrl = @"\images\" + fileName + extension;
                _mapper.Map(imageDto, image);
                _unitOfWork.Image.Add(image);
                await _unitOfWork.SaveAsync();
                return Ok(image);
            }
            else
            {
                return BadRequest("Bạn cần nhập ảnh.");
            }
            
        }

        [HttpPut]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<Image>> UpdateImage([FromForm]ImageDTO imageDto, int id)
        {
            var objFromDb = await _unitOfWork.Image.GetAsync(id);
            if(objFromDb != null)
            {
                string fileName = Guid.NewGuid().ToString();
                var extension = Path.GetExtension(imageDto.File.FileName);
                if (objFromDb.ImageUrl != null)
                {
                    var oldImagePath = objFromDb.ImageUrl.TrimStart('\\');
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                using (var fileStream = new FileStream(Path.Combine(@"images", fileName + extension), FileMode.Create))
                {
                    imageDto.File.CopyTo(fileStream);
                }
                objFromDb.ImageUrl = @"\images\" + fileName + extension;
                _mapper.Map(imageDto, objFromDb);
                _unitOfWork.Image.Update(objFromDb);
                await _unitOfWork.SaveAsync();
                return Ok(objFromDb);
            }
            return NotFound("Không tìm thấy ảnh để cập nhật");
            
        }

        [HttpDelete]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<Image>> DeleteImage(int id)
        {
            var result = _unitOfWork.Image.Delete(id);
            if (result == true)
            {
                await _unitOfWork.SaveAsync();
                return Ok();
            }
            return BadRequest("Không thể xoá ảnh.");
        }
    }
}
