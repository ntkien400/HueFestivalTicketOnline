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
            var objFromDb = await _unitOfWork.Image.GetAsync(id);
            if (objFromDb != null)
            {
                return Ok(objFromDb);
            }
            return NotFound("Image not exists");
        }

        [HttpGet("get-all-images")]
        [AllowAnonymous]
        public async Task<ActionResult> GetImages()
        {
            var objs = await _unitOfWork.Image.GetAllAsync();
            return Ok(objs);
        }

        [HttpGet("get-image-by-program")]
        [AllowAnonymous]
        public async Task<ActionResult> GetImagesByProgramId(int id)
        {
            var objs = await _unitOfWork.Image.GetAllAsync(o => o.FesProgramId == id);
            if(objs !=null)
            {
                return Ok(objs);
            }
            return NotFound("Can't find image");
        }

        
        [HttpPost]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult> AddImage([FromForm]ImageDTO imageDto)
        {
            if(imageDto.File != null && imageDto.FesProgramId != null)
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
                var result = await _unitOfWork.SaveAsync();
                if(result > 0)
                {
                    return Ok(image);
                }
                return BadRequest("Something wrong when adding");
            }
            else
            {
                return BadRequest("You must fill all field");
            }
            
        }

        [HttpPut]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<Image>> UpdateImage([FromForm]ImageDTO imageDto, int id)
        {
            var objFromDb = await _unitOfWork.Image.GetAsync(id);
            if(objFromDb != null)
            {
                if(imageDto.File != null)
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
                }
                
                _mapper.Map(imageDto, objFromDb);
                _unitOfWork.Image.Update(objFromDb);
                var result = await _unitOfWork.SaveAsync();
                if(result > 0)
                {
                    return Ok(objFromDb);
                }
                return BadRequest("Something wrong when updating");
            }
            return NotFound("Can't find image to update");
            
        }

        [HttpDelete]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<Image>> DeleteImage(int id)
        {

            var image = await _unitOfWork.Image.GetAsync(id);
            if (image != null)
            {
                _unitOfWork.Image.Delete(image);
                var result = await _unitOfWork.SaveAsync();
                if (result > 0)
                {
                    return Ok("Delete successfully");
                }
                return BadRequest("Something wrong when deleting");
            }
            return BadRequest("Can't find image to delete");
        }
    }
}
