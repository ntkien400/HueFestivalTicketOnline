using HueFestivalTicketOnline.DataAccess.Data;
using HueFestivalTicketOnline.DataAccess.Repository.IRepository;
using HueFestivalTicketOnline.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace HueFestivalTicketOnline.DataAccess.Repository
{
    public class HistoryCheckRepository : GenericRepository<HistoryCheck>, IHistoryCheckRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public HistoryCheckRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        
    }
}
