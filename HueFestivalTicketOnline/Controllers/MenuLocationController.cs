using AutoMapper;
using HueFestivalTicketOnline.DataAccess.Repository.IRepository;
using HueFestivalTicketOnline.DTOs;
using HueFestivalTicketOnline.DTOs.Authentiction;
using HueFestivalTicketOnline.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HueFestivalTicketOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuLocationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MenuLocationController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<MenuLocationDTO>> GetMenuLocation(int id)
        {
            MenuLocationDTO menuLocation = new MenuLocationDTO();
            var menu =  await _unitOfWork.MenuLocation.GetAsync(id);
            var submenus = await _unitOfWork.SubMenuLocation.GetAllAsync(s => s.MenuLocationId == id);
            menuLocation.MenuId = menu.Id;
            menuLocation.MenuTitle = menu.MenuTitle;
            foreach(SubMenuLocation sub in submenus)
            {
                menuLocation.SubMenus.Add(_mapper.Map<SubMenuLocation, ViewSubMenuLocation>(sub));
            }
            return Ok(menuLocation);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<MenuLocation>>> GetMenuLocations()
        {
            var objs = await _unitOfWork.MenuLocation.GetAllAsync();
            return Ok(objs);
        }

        [HttpPost]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<MenuLocation>> AddMenuLocation(MenuLocation menuLocation)
        {
            _unitOfWork.MenuLocation.Add(menuLocation);
            await _unitOfWork.SaveAsync();
            return Ok(menuLocation);
        }

        [HttpPut]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<MenuLocation>> UpdateMenuLocation(MenuLocation menuLocation)
        {
            var objfromDb = await _unitOfWork.MenuLocation.GetAsync(menuLocation.Id);
            if(objfromDb != null)
            {
                _unitOfWork.MenuLocation.Update(menuLocation);
                await _unitOfWork.SaveAsync();
                return Ok(menuLocation);
            }
            return NotFound("Không tìm thấy dữ liệu.");
        }

        [HttpDelete]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<MenuLocation>> DeleteMenuLocation(int id)
        {
            var result = _unitOfWork.MenuLocation.Delete(id);
            if(result == true)
            {
                await _unitOfWork.SaveAsync();
                return Ok();
            }
            return BadRequest("Không tìm thấy dữ liệu để xoá.");
        }
    }
}
