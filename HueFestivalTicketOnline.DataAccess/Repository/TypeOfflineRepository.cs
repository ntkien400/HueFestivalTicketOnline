using HueFestivalTicketOnline.DataAccess.Data;
using HueFestivalTicketOnline.DataAccess.Repository.IRepository;
using HueFestivalTicketOnline.Models.Models;

namespace HueFestivalTicketOnline.DataAccess.Repository
{
    public class TypeOfflineRepository : GenericRepository<TypeOffline>, ITypeOfflineRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TypeOfflineRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        

    }
}
