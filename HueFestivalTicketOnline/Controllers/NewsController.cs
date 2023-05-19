using AutoMapper;
using HueFestivalTicketOnline.DataAccess.Repository.IRepository;
using HueFestivalTicketOnline.DTOs;
using HueFestivalTicketOnline.DTOs.Authentiction;
using HueFestivalTicketOnline.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HueFestivalTicketOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NewsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<News>> GetNews(int id)
        {
            var subMenu =  await _unitOfWork.News.GetAsync(id);
            if(subMenu != null) 
            {
                return Ok(subMenu);
            }
            return NotFound("Data is not exist");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<News>>> GetAllNews(int? page_index, int? page_size)
        {
            if(page_size != null && page_index != null)
            {
                int takeobjs = page_size.Value;
                int skipobjs = page_size.Value * (page_index.Value - 1);
                var objs = await _unitOfWork.News.GetAllAsync(take: takeobjs, skip: skipobjs);
                return Ok(objs);
            }
            else
            {
                return Ok(new List<News>());
            }
            
            
        }

        [HttpPost]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<News>> AddNews([FromForm]CreateNewsDTO newsDto)
        {
            
            if (newsDto.File != null && newsDto.Content != null  && newsDto.Title != null)
            {
                var news = new News();
                string fileName = Guid.NewGuid().ToString();
                var extension = Path.GetExtension(newsDto.File.FileName);

                using (var fileStream = new FileStream(Path.Combine(@"images", fileName + extension),FileMode.Create))
                {
                    newsDto.File.CopyTo(fileStream);
                }

                news.ImageUrl = @"\images\" + fileName + extension;
                news.DateCreated = DateTime.Now;
                var AccountId = HttpContext.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value;
                news.AccountId = AccountId;
                _mapper.Map(newsDto, news);
                _unitOfWork.News.Add(news);
                await _unitOfWork.SaveAsync();
                return Ok(news);
            }
            else
            {
                return BadRequest("You must fill all information");
            }
            
        }

        [HttpPut]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<News>> UpdateNews([FromForm]CreateNewsDTO newsDto, int id)
        {
            var objFromDb = await _unitOfWork.News.GetAsync(id);
           
            if(objFromDb != null)
            {
                if(newsDto.File != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var extension = Path.GetExtension(newsDto.File.FileName);
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
                        newsDto.File.CopyTo(fileStream);
                    }

                    objFromDb.ImageUrl = @"\images\" + fileName + extension;
                }
                objFromDb.DateChanged = DateTime.Now;
                _mapper.Map(newsDto, objFromDb);
                _unitOfWork.News.Update(objFromDb);
                await _unitOfWork.SaveAsync();
                return Ok(objFromDb);
            }

            return BadRequest("Something wrong when upadte data");
        }

        [HttpDelete]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<News>> DeleteNews(int id)
        {
            var result = _unitOfWork.News.Delete(id);
            if(result == true)
            {
                await _unitOfWork.SaveAsync();
                return Ok();
            }
            return BadRequest("Some thing wrong when delete");
        }
    }
}
