using HueFestivalTicketOnline.DataAccess.Data;
using HueFestivalTicketOnline.DataAccess.Repository.IRepository;
using HueFestivalTicketOnline.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace HueFestivalTicketOnline.DataAccess.Repository
{
    public class FesTypeTicketRepository : GenericRepository<FesTypeTicket>, IFesTypeTicketRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public FesTypeTicketRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public bool CheckTicketAmount(FesTypeTicket fesTypeTicket)
        {
            if(fesTypeTicket.Quanity >0)
                return true;
            return false;
        }

        public bool UpdateTicketAmount(FesTypeTicket fesTypeTicket, int quantity)
        {
            if(fesTypeTicket.Quanity >= quantity)
            {
                fesTypeTicket.Quanity -= quantity;
                _dbContext.FesTypeTickets.Update(fesTypeTicket);
                return true;
            }
            return false;
        }
    }
}
