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
        public async Task<ActionResult<SubMenuLocation>> GetSubMenuLocation(int id)
        {
            var subMenu =  await _unitOfWork.SubMenuLocation.GetAsync(id);
            if(subMenu != null) 
            {
                return Ok(subMenu);
            }
            return NotFound("Submenu location not exists");
        }

        [HttpGet("get-all-submenu")]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<List<ViewSubMenuLocation>>> GetSubMenuLocations()
        {
            var objs = await _unitOfWork.SubMenuLocation.GetAllAsync();
            List<ViewSubMenuLocation> subMenus = new List<ViewSubMenuLocation>();
            if(objs != null)
            {
                foreach (var obj in objs)
                {
                    subMenus.Add(_mapper.Map<SubMenuLocation, ViewSubMenuLocation>(obj));
                }
                return Ok(subMenus);
            }
            return Ok(subMenus);
        }

        [HttpGet("get-submenu-list")]
        [AllowAnonymous]
        public async Task<ActionResult<List<ViewSubMenuLocation>>> GetSubMenuByMenu(int menuId)
        {
            var listSubMenu = await _unitOfWork.SubMenuLocation.GetAllAsync(s => s.MenuLocationId == menuId);
            List<ViewSubMenuLocation> subMenus = new List<ViewSubMenuLocation>();
            if (listSubMenu != null)
            {
                foreach (var obj in listSubMenu)
                {
                    subMenus.Add(_mapper.Map<SubMenuLocation, ViewSubMenuLocation>(obj));
                }
                return Ok(subMenus);
            }
            return Ok(subMenus);
        }

        [HttpPost]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<SubMenuLocation>> AddSubMenuLocation([FromForm]SubMenuLocationDTO subMenuLocation)
        {
            if(subMenuLocation.CateName !=null && subMenuLocation.MenuLocationId != null)
            {
                if (subMenuLocation.File != null)
                {
                    var subMenu = new SubMenuLocation();
                    string fileName = Guid.NewGuid().ToString();
                    var extension = Path.GetExtension(subMenuLocation.File.FileName);

                    using (var fileStream = new FileStream(Path.Combine(@"images", fileName + extension), FileMode.Create))
                    {
                        subMenuLocation.File.CopyTo(fileStream);
                    }

                    subMenu.ImageUrl = @"\images\" + fileName + extension;
                    _mapper.Map(subMenuLocation, subMenu);
                    _unitOfWork.SubMenuLocation.Add(subMenu);
                    var result = await _unitOfWork.SaveAsync();
                    if (result > 0)
                    {
                        return Ok(subMenu);
                    }
                    return BadRequest("Something wrong when adding");
                }
                return BadRequest("You must import image");
            }
            return BadRequest("You must fill all field");
        }

        [HttpPut]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<SubMenuLocation>> UpdateSubMenuLocation([FromForm]SubMenuLocationDTO subMenuLocation, int id)
        {
            var objFromDb = await _unitOfWork.SubMenuLocation.GetAsync(id);
           
            if(objFromDb != null)
            {
                if(subMenuLocation.File != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var extension = Path.GetExtension(subMenuLocation.File.FileName);
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
                        subMenuLocation.File.CopyTo(fileStream);
                    }

                    objFromDb.ImageUrl = @"\images\" + fileName + extension;
                }
                _mapper.Map(subMenuLocation, objFromDb);
                _unitOfWork.SubMenuLocation.Update(objFromDb);
                var result = await _unitOfWork.SaveAsync();
                if (result > 0)
                {
                    return Ok("Update successfully");
                }
                return BadRequest("Something wrong when updating");
            }
            return NotFound("Can't find sub menu to update");
        }

        [HttpDelete]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<SubMenuLocation>> DeleteSubMenuLocation(int id)
        {
            var subMenuLocation = await _unitOfWork.SubMenuLocation.GetAsync(id);
            if(subMenuLocation != null)
            {
                _unitOfWork.SubMenuLocation.Delete(subMenuLocation);
                var result = await _unitOfWork.SaveAsync();
                if (result > 0)
                {
                    return Ok("Delete successfully");
                }
                return BadRequest("Something wrong when deleting");
            }
            return NotFound("Can't find sub menu to delete");
        }
    }
}
