using HueFestivalTicketOnline.DataAccess.Data;
using HueFestivalTicketOnline.DataAccess.Repository.IRepository;
using HueFestivalTicketOnline.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace HueFestivalTicketOnline.DataAccess.Repository
{
    public class MenuLocationRepository : GenericRepository<MenuLocation>, IMenuLocationRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public MenuLocationRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

    }
}
