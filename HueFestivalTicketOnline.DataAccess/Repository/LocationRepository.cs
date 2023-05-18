using HueFestivalTicketOnline.DataAccess.Data;
using HueFestivalTicketOnline.DataAccess.Repository.IRepository;
using HueFestivalTicketOnline.Models.Models;

namespace HueFestivalTicketOnline.DataAccess.Repository
{
    public class LocationRepository : GenericRepository<Location>, ILocationRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public LocationRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        

    }
}
