using AutoMapper;
using HueFestivalTicketOnline.DataAccess.Repository.IRepository;
using HueFestivalTicketOnline.DTOs;
using HueFestivalTicketOnline.DTOs.Authentiction;
using HueFestivalTicketOnline.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HueFestivalTicketOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubMenuLocationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SubMenuLocationController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ViewSubMenuLocation>> GetSubMenuLocation(int id)
        {
            var subMenu =  await _unitOfWork.SubMenuLocation.GetAsync(id);
            if(subMenu != null) 
            {
                return Ok(_mapper.Map<SubMenuLocation, ViewSubMenuLocation>(subMenu));
            }
            return NotFound("Không tìm thấy dữ liệu.");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<ViewSubMenuLocation>>> GetSubMenuLocations()
        {
            var objs = await _unitOfWork.SubMenuLocation.GetAllAsync();
            List<ViewSubMenuLocation> subMenus = new List<ViewSubMenuLocation>();
            foreach(var obj in objs)
            {
                subMenus.Add(_mapper.Map<SubMenuLocation, ViewSubMenuLocation>(obj));
            }
            return Ok(subMenus);
        }

        [HttpPost]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<SubMenuLocation>> AddSubMenuLocation([FromForm]SubMenuLocationDTO subMenuLocation)
        {
            
            if (subMenuLocation.File != null)
            {
                var subMenu = new SubMenuLocation();
                string fileName = Guid.NewGuid().ToString();
                var extension = Path.GetExtension(subMenuLocation.File.FileName);

                using (var fileStream = new FileStream(Path.Combine(@"images", fileName + extension),FileMode.Create))
                {
                    subMenuLocation.File.CopyTo(fileStream);
                }

                subMenu.ImageUrl = @"\images\" + fileName + extension;
                _mapper.Map(subMenuLocation, subMenu);
                _unitOfWork.SubMenuLocation.Add(subMenu);
                await _unitOfWork.SaveAsync();
                return Ok(subMenuLocation);
            }
            else
            {
                return BadRequest("Bạn phải nhập ảnh");
            }
            
        }

        [HttpPut]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<SubMenuLocation>> UpdateSubMenuLocation([FromForm]SubMenuLocationDTO subMenuLocation, int id)
        {
            var objFromDb = await _unitOfWork.SubMenuLocation.GetAsync(id);
           
            if(objFromDb != null)
            {
                string fileName = Guid.NewGuid().ToString();
                var extension = Path.GetExtension(subMenuLocation.File.FileName);
                if (objFromDb.ImageUrl != null)
                {
                    var oldImagePath = objFromDb.ImageUrl.TrimStart('\\');
                    if(System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                using (var fileStream = new FileStream(Path.Combine(@"images", fileName + extension), FileMode.Create))
                {
                    subMenuLocation.File.CopyTo(fileStream);
                }

                objFromDb.ImageUrl = @"\images\" + fileName + extension;
                _mapper.Map(subMenuLocation, objFromDb);
                _unitOfWork.SubMenuLocation.Update(objFromDb);
                await _unitOfWork.SaveAsync();
                return Ok(objFromDb);
            }

            return BadRequest("Không thể cập nhật dữ liệu");
        }

        [HttpDelete]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<SubMenuLocation>> DeleteSubMenuLocation(int id)
        {
            var result = _unitOfWork.SubMenuLocation.Delete(id);
            if(result == true)
            {
                await _unitOfWork.SaveAsync();
                return Ok();
            }
            return BadRequest();
        }
    }
}
