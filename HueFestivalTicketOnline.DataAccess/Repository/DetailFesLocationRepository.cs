using HueFestivalTicketOnline.DataAccess.Data;
using HueFestivalTicketOnline.DataAccess.Repository.IRepository;
using HueFestivalTicketOnline.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace HueFestivalTicketOnline.DataAccess.Repository
{
    public class DetailFesLocationRepository : GenericRepository<DetailFesLocation>, IDetailFesLocationRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DetailFesLocationRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        
    }
}
