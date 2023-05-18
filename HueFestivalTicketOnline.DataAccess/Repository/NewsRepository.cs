using HueFestivalTicketOnline.DataAccess.Data;
using HueFestivalTicketOnline.DataAccess.Repository.IRepository;
using HueFestivalTicketOnline.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace HueFestivalTicketOnline.DataAccess.Repository
{
    public class NewsRepository : GenericRepository<News>, INewsRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public NewsRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        
    }
}
