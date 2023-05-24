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
    public class MenuLocationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MenuLocationController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("get-menu")]
        [AllowAnonymous]
        public async Task<ActionResult<MenuLocationDTO>> GetMenuLocation(int id)
        {
            MenuLocationDTO menuLocation = new MenuLocationDTO();
            var menu = await _unitOfWork.MenuLocation.GetAsync(id);
            if(menu != null)
            {
                menuLocation.MenuId = menu.Id;
                menuLocation.MenuTitle = menu.MenuTitle;
                return Ok(menuLocation);
            }
            return NotFound("Menu location not exists");
        }

        [HttpGet("get-all-menu")]
        [AllowAnonymous]
        public async Task<ActionResult<List<MenuLocation>>> GetMenuLocations()
        {
            var objs = await _unitOfWork.MenuLocation.GetAllAsync();
            return Ok(objs);
        }

        [HttpPost]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<MenuLocation>> AddMenuLocation([FromForm]MenuLocation menuLocation)
        {
            _unitOfWork.MenuLocation.Add(menuLocation);
            var result = await _unitOfWork.SaveAsync();
            if (result > 0)
            {
                return Ok(menuLocation);
            }
            return BadRequest("Something wrong when adding");
        }

        [HttpPut]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<MenuLocation>> UpdateMenuLocation([FromForm]MenuLocation menuLocation)
        {
            var objfromDb = await _unitOfWork.MenuLocation.GetAsync(menuLocation.Id);
            if(objfromDb != null)
            {
                objfromDb.MenuTitle = menuLocation.MenuTitle;
                _unitOfWork.MenuLocation.Update(objfromDb);
                var result = await _unitOfWork.SaveAsync();
                if (result > 0)
                {
                    return Ok("Update successfully");
                }
                return BadRequest("Something wrong when updating");
            }
            return NotFound("Menu is not exists");
        }

        [HttpDelete]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<MenuLocation>> DeleteMenuLocation(int id)
        {
            var menuLocation = await _unitOfWork.MenuLocation.GetAsync(id);
            if(menuLocation != null)
            {
                _unitOfWork.MenuLocation.Delete(menuLocation);
                var result = await _unitOfWork.SaveAsync();
                if (result > 0)
                {
                    return Ok("Delete successfully");
                }
                return BadRequest("Something wrong when deleting");
            }
            return NotFound("Menu is not exists");
        }
    }
}
