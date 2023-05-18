using HueFestivalTicketOnline.DataAccess.Data;
using HueFestivalTicketOnline.DataAccess.Repository.IRepository;
using HueFestivalTicketOnline.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace HueFestivalTicketOnline.DataAccess.Repository
{
    public class ImageRepository : GenericRepository<Image>, IImageRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ImageRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Image> GetImageByName(string name)
        {
            return await _dbContext.Images.Where(i => i.ImageUrl.Substring(8,36) == name).FirstOrDefaultAsync();
        }
    }
}
