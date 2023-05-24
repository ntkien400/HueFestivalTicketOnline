using AutoMapper;
using HueFestivalTicketOnline.DataAccess.Repository.IRepository;
using HueFestivalTicketOnline.Models.DTOs;
using HueFestivalTicketOnline.Models.DTOs.Authentiction;
using HueFestivalTicketOnline.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HueFestivalTicketOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AboutInformationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AboutInformationController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("get-about-detail")]
        [AllowAnonymous]
        public async Task<ActionResult<AboutInformation>> GetAboutInformation(int id)
        {
            var detailFes = await _unitOfWork.AboutInformation.GetAsync(id); 
            if(detailFes == null)
            {
                return NotFound("Data not exists");
            }
            return Ok(detailFes);
        }

        [HttpGet("get-about-list")]
        [AllowAnonymous]
        public async Task<ActionResult<List<AboutInformationDTO>>> GetAboutInformations()
        {
            var objs = await _unitOfWork.AboutInformation.GetAllAsync();
            return Ok(objs);
        }

        [HttpPost]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<AboutInformationDTO>> AddAboutInformation(AboutInformationDTO aboutDto)
        {
            var aboutInformation = new AboutInformation();
            var AccountId = HttpContext.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value;

            if(aboutDto.AboutTitle!= null && aboutDto.AboutContent != null)
            {
                _mapper.Map(aboutDto, aboutInformation);
                aboutInformation.DateCreated = DateTime.Now;
                aboutInformation.DateChanged = DateTime.Now;
                aboutInformation.AccountId = AccountId;

                _unitOfWork.AboutInformation.Add(aboutInformation);
                var result = await _unitOfWork.SaveAsync();
                if(result > 0)
                {
                    return Ok(aboutInformation);
                }
                return BadRequest("Something wrong when adding");
            }
            return BadRequest("You nust fill all field");
        }

        [HttpPut("edit-about")]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<AboutInformationDTO>> UpdateAboutInformation(AboutInformationDTO aboutDto, int id)
        {
            var objFromDb = await _unitOfWork.AboutInformation.GetAsync(id);
            if(objFromDb != null)
            {
                _mapper.Map(aboutDto, objFromDb);
                _unitOfWork.AboutInformation.Update(objFromDb);
                var result = await _unitOfWork.SaveAsync();
                if (result > 0)
                {
                    return Ok("Update successfully");
                }
                return BadRequest("Something wrong when updating");
            }
            return NotFound("Can't find to update");
            
        }

        [HttpDelete("delete-about")]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<AboutInformation>> DeleteAboutInformation(int id)
        {
            var aboutInformation = await _unitOfWork.AboutInformation.GetAsync(id);
            if(aboutInformation !=null)
            {
                _unitOfWork.AboutInformation.Delete(aboutInformation);
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
