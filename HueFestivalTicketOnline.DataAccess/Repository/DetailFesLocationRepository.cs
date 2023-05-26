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

        public bool CheckDate(string s1, string s2)
        {
            var date1 = DateOnly.ParseExact(s1, "yyyy-MM-dd");
            var date2 = DateOnly.ParseExact(s2, "yyyy-MM-dd");
            var now = DateTime.Now;
            if (date1 < DateOnly.FromDateTime(now))
                return false;
            if (date1 > date2)
                return false;
            return true;
        }
    }
}
