using HueFestivalTicketOnline.DataAccess.Data;
using HueFestivalTicketOnline.DataAccess.Repository.IRepository;
using HueFestivalTicketOnline.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace HueFestivalTicketOnline.DataAccess.Repository
{
    public class SubMenuLocationRepository : GenericRepository<SubMenuLocation>, ISubMenuLocationRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SubMenuLocationRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

       /* public async Task<SubMenuLocation> GetSubMenuLocationAsync(int id)
        {
            return await _dbContext.SubMenuLocations
                .Include(i => i.Image)
                .FirstOrDefaultAsync(s => s.CateId == id);
        }

        public async Task<IReadOnlyList<SubMenuLocation>> GetSubMenuLocationsAsync()
        {
            return await _dbContext.SubMenuLocations
                .Include(i => i.Image)
                .ToListAsync();
        }*/
    }
}
