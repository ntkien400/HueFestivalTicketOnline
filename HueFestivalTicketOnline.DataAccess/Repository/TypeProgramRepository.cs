using HueFestivalTicketOnline.DataAccess.Data;
using HueFestivalTicketOnline.DataAccess.Repository.IRepository;
using HueFestivalTicketOnline.Models.Models;

namespace HueFestivalTicketOnline.DataAccess.Repository
{
    public class TypeProgramRepository : GenericRepository<TypeProgram>, ITypeProgramRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TypeProgramRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        

    }
}
