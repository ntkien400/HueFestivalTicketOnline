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
    public class LocationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LocationController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ViewLocation>> GetLocation(int id)
        {
            var location =  await _unitOfWork.Location.GetAsync(id);
            if(location !=null)
            {
                return Ok(_mapper.Map<Location, ViewLocation>(location));
            }
            return NotFound("Location not exist");
        }

        [HttpGet("get-all-location")]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<List<ViewLocation>>> GetLocations(int? page_index, int? page_size)
        {
            if (page_index != null && page_size != null)
            {
                int takeobjs = page_size.Value;
                int skipobjs = page_size.Value * (page_index.Value - 1);
                var objFromDb = await _unitOfWork.Location.GetAllAsync(take: takeobjs, skip: skipobjs);
                List<ViewLocation> locations = new List<ViewLocation>();
                foreach (var obj in objFromDb)
                {
                    locations.Add(_mapper.Map<Location, ViewLocation>(obj));
                }
                return Ok(locations);
            }
            else
            {
                return Ok(new List<ViewLocation>());
            }
        }

        [HttpGet("get-location-list")]
        [AllowAnonymous]
        public async Task<ActionResult<List<ViewLocation>>> GetLocationsBySubMenu(int subMenuId, int? page_index, int? page_size)
        {
            var subMenu = await _unitOfWork.SubMenuLocation.GetAsync(subMenuId);
            if(subMenu != null)
            {
                if(page_index != null && page_size != null)
                {
                    int takeobjs = page_size.Value;
                    int skipobjs = page_size.Value * (page_index.Value - 1);
                    var objFromDb = await _unitOfWork.Location.GetAllAsync(o => o.SubMenuLocationId == subMenuId, take: takeobjs, skip: skipobjs);
                    List<ViewLocation> locations = new List<ViewLocation>();
                    foreach (var obj in objFromDb)
                    {
                        locations.Add(_mapper.Map<Location, ViewLocation>(obj));
                    }
                    return Ok(locations);
                }
                else
                {
                    return Ok(new List<ViewLocation>());
                }
            }
            return NotFound("Sub menu is not exists");
        }

        [HttpPost]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<CreateLocationDTO>> AddLocation([FromForm]CreateLocationDTO locationDto)
        {
            if (locationDto.File != null 
                && locationDto.LocationName != null
                && locationDto.Description != null
                && locationDto.SubMenuLocationId != null)
            {
                var location = new Location();
                string fileName = Guid.NewGuid().ToString();
                var extension = Path.GetExtension(locationDto.File.FileName);

                using (var fileStream = new FileStream(
                    Path.Combine(@"images", fileName + extension),
                    FileMode.Create))
                {
                    locationDto.File.CopyTo(fileStream);
                }
                location.ImageUrl = @"\images\" + fileName + extension;
                _mapper.Map(locationDto, location);
                _unitOfWork.Location.Add(location);
                var result = await _unitOfWork.SaveAsync();
                if(result > 0)
                {
                    return Ok(location);
                }
                return BadRequest("Something wrong when adding");

            }
            else
            {
                return BadRequest("You must fill all information");
            }
            
        }

        [HttpPut]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<Location>> UpdateLocation([FromForm]CreateLocationDTO locationDto, int id)
        {
            var objFromDb = await _unitOfWork.Location.GetAsync(id);

            if (objFromDb != null)
            {
                if(locationDto.File != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var extension = Path.GetExtension(locationDto.File.FileName);
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
                        locationDto.File.CopyTo(fileStream);
                    }
                    objFromDb.ImageUrl = @"\images\" + fileName + extension;
                }
                _mapper.Map(locationDto, objFromDb);
                _unitOfWork.Location.Update(objFromDb);
                var result = await _unitOfWork.SaveAsync();

                if(result > 0)
                {
                    return Ok(objFromDb);
                }
                return BadRequest("Something wrong when updating");
                
            }

            return NotFound("Can't find location to update");

        }

        [HttpDelete]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<Location>> DeleteLocation(int id)
        {
            var location = await _unitOfWork.Location.GetAsync(id);
            if (location != null)
            {
                _unitOfWork.Location.Delete(location);
                var result =  await _unitOfWork.SaveAsync();
                if(result > 0)
                {
                    return Ok("Delete successfully");
                }
                return BadRequest("Something wrong when deleting");
            }
            return NotFound("Can't find location to delete");
        }
    }
}
